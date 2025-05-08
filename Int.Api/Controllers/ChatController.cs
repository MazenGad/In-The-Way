using Int.Domain.DTOs.Auth;
using Int.Domain.Entities;
using Int.Domain.Entities.Chat;
using Int.Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Int.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly FiElSekkaContext _context;

		public ChatController(UserManager<User> userManager, FiElSekkaContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		// الحصول على الرسائل بين اليوزر والادمن
		[HttpGet("messages")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> GetMessages()
		{
			var currentUserId = _userManager.GetUserId(User);
			var currentUser = await _userManager.FindByIdAsync(currentUserId);
			var userRole = (await _userManager.GetRolesAsync(currentUser)).FirstOrDefault();

			if (userRole == "User")
			{
				var users = await _userManager.Users.ToListAsync();
				var adminUser = null as User;

				foreach (var user in users)
				{
					var roles = await _userManager.GetRolesAsync(user); // الحصول على الأدوار بشكل غير متزامن
					if (roles.Contains("Admin"))
					{
						adminUser = user;
						break;
					}
				}


				if (adminUser != null)
				{
					// إظهار المحادثة بين هذا اليوزر و الادمن فقط
					var chats = await _context.Chats
						.Where(c => (c.SenderId == currentUserId && c.ReceiverId == adminUser.Id) ||
									(c.SenderId == adminUser.Id && c.ReceiverId == currentUserId))
						.OrderBy(c => c.Timestamp)
						.Select(c => new
						{
							c.Id,
							c.SenderId,
							c.ReceiverId,
							c.Message,
							c.Timestamp,
						})
						.ToListAsync();
					return Ok(chats);
				}
			}
			else if (userRole == "Admin")
			{
				// إظهار المحادثات مع كل اليوزرز فقط (المحادثات بين الـ Admin والـ User)
				var chats = await _context.Chats
					.Where(c => (c.ReceiverId == currentUserId || c.SenderId == currentUserId))  // المحادثات التي تخص الادمن فقط
					.GroupBy(c => new { c.SenderId, c.ReceiverId }) // تجميع الرسائل حسب المرسل والمستقبل (لتفصل بين المحادثات)
					.Select(group => new
					{
						UserId = group.Key.SenderId == currentUserId ? group.Key.ReceiverId : group.Key.SenderId, // تعيين الـ UserId الصحيح
						Messages = group
							.OrderBy(c => c.Timestamp) // ترتيب الرسائل حسب الوقت
							.Select(c => new
							{
								c.Id,
								c.Message,
								c.Timestamp,
								Direction = c.SenderId == currentUserId ? "Sent" : "Received" // إضافة اتجاه الرسالة (مرسلة أو مستلمة)
							})
							.ToList()
					})
					.ToListAsync();

				// نعيد المحادثات بحيث تظهر الرسائل المرسلة والمستلمة معًا بنفس الـ userId
				var result = chats
					.GroupBy(chat => chat.UserId)
					.Select(group => new
					{
						UserId = group.Key,
						SentMessages = group.SelectMany(chat => chat.Messages)
										   .Where(message => message.Direction == "Sent")
										   .ToList(),
						ReceivedMessages = group.SelectMany(chat => chat.Messages)
												.Where(message => message.Direction == "Received")
												.ToList()
					})
					.ToList();

				return Ok(result);
			}

			return BadRequest("No chats found.");
		}

		//// إرسال رسالة
		//[HttpPost("send")]
		//[Authorize]
		//public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest model)
		//{
		//	var currentUserId = _userManager.GetUserId(User);
		//	var currentUser = await _userManager.FindByIdAsync(currentUserId);
		//	var userRole = (await _userManager.GetRolesAsync(currentUser)).FirstOrDefault();

		//	if (string.IsNullOrEmpty(model.Message))
		//		return BadRequest("Message cannot be empty.");

		//	// إذا كان اليوزر هو من يرسل الرسالة
		//	if (userRole == "User")
		//	{
		//		var users = await _userManager.Users.ToListAsync();
		//		var adminUser = null as User;

		//		foreach (var user in users)
		//		{
		//			var roles = await _userManager.GetRolesAsync(user); // الحصول على الأدوار بشكل غير متزامن
		//			if (roles.Contains("Admin"))
		//			{
		//				adminUser = user;
		//				break;
		//			}
		//		}
		//		if (adminUser != null)
		//		{
		//			var chat = new Chat
		//			{
		//				SenderId = currentUserId,
		//				ReceiverId = adminUser.Id,
		//				Message = model.Message,
		//				Timestamp = DateTime.Now
		//			};
		//			_context.Chats.Add(chat);
		//			await _context.SaveChangesAsync();
		//			return Ok(new { success = true });
		//		}
		//	}

		//	// إذا كان الادمن هو من يرسل الرسالة
		//	if (userRole == "Admin")
		//	{
		//		var user = await _userManager.Users
		//			.FirstOrDefaultAsync(u => u.Id == model.UserId);
		//		if (user != null)
		//		{
		//			var chat = new Chat
		//			{
		//				SenderId = currentUserId,
		//				ReceiverId = user.Id,
		//				Message = model.Message,
		//				Timestamp = DateTime.Now
		//			};
		//			_context.Chats.Add(chat);
		//			await _context.SaveChangesAsync();
		//			return Ok(new { success = true });
		//		}
		//	}

		//	return BadRequest("Failed to send message.");
		//}

		[HttpPost("send/user-to-admin")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> SendMessageFromUserToAdmin([FromBody] string Message)
		{
			var currentUserId = _userManager.GetUserId(User);
			var currentUser = await _userManager.FindByIdAsync(currentUserId);
			var userRole = (await _userManager.GetRolesAsync(currentUser)).FirstOrDefault();

			if (string.IsNullOrEmpty(Message))
				return BadRequest("Message cannot be empty.");

			// إذا كان اليوزر هو من يرسل الرسالة
			if (userRole == "User")
			{
				var users = await _userManager.Users.ToListAsync();
				var adminUser = null as User;

				foreach (var user in users)
				{
					var roles = await _userManager.GetRolesAsync(user); // الحصول على الأدوار بشكل غير متزامن
					if (roles.Contains("Admin"))
					{
						adminUser = user;
						break;
					}
				}
				if (adminUser != null)
				{
					var chat = new Chat
					{
						SenderId = currentUserId,
						ReceiverId = adminUser.Id,
						Message = Message,
						Timestamp = DateTime.Now
					};
					_context.Chats.Add(chat);
					await _context.SaveChangesAsync();
					return Ok(new { success = true });
				}
			}

			return BadRequest("Failed to send message.");
		}

		[HttpPost("send/admin-to-user")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
		public async Task<IActionResult> SendMessageFromAdminToUser([FromBody] SendMessageRequest model)
		{
			var currentUserId = _userManager.GetUserId(User);
			var currentUser = await _userManager.FindByIdAsync(currentUserId);
			var userRole = (await _userManager.GetRolesAsync(currentUser)).FirstOrDefault();

			if (string.IsNullOrEmpty(model.Message) || string.IsNullOrEmpty(model.UserId))
				return BadRequest("Message and UserId cannot be empty.");

			// إذا كان الادمن هو من يرسل الرسالة
			if (userRole == "Admin")
			{
				var user = await _userManager.Users
					.FirstOrDefaultAsync(u => u.Id == model.UserId);
				if (user != null)
				{
					var chat = new Chat
					{
						SenderId = currentUserId,
						ReceiverId = user.Id,
						Message = model.Message,
						Timestamp = DateTime.Now
					};
					_context.Chats.Add(chat);
					await _context.SaveChangesAsync();
					return Ok(new { success = true });
				}
			}

			return BadRequest("Failed to send message.");
		}


	}
}
