using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Int.Api.Hubs
{
	public class ChatHub : Hub
	{
		// إرسال رسالة بين اليوزر والأدمن فقط
		public async Task SendMessage(string user, string message)
		{
			Console.WriteLine($"Message from {user}: {message}");
			// تحقق من أن الشخص الذي يرسل الرسالة هو الـ User
			if (Context.User.Identity.Name == "User")
			{
				// يرسل الرسالة إلى الأدمن فقط
				await Clients.User("Admin").SendAsync("ReceiveMessage", user, message);
			}
			else if (Context.User.Identity.Name == "Admin")
			{
				// لو كان الأدمن هو الذي يرسل، يرسل الرسالة إلى اليوزر فقط
				await Clients.User("User").SendAsync("ReceiveMessage", user, message);
			}
		}

		// عند الاتصال، التحقق من اسم المستخدم
		public override Task OnConnectedAsync()
		{
			var user = Context.User.Identity.Name; // الحصول على اسم المستخدم من الـ Context
			if (user == "Admin")
			{
				// لو الـ Admin اتصل، نقدر نضيف منطق خاص به هنا
			}
			return base.OnConnectedAsync();
		}
	}
}
