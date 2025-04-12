using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Int.Domain;
using Int.Domain.Repositories.Contract;
using Int.Infrastructure.Entities;
using InT.Infrastructure.Repositories;

namespace InT.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FiElSekkaContext _context;
        private readonly Dictionary<string, object> _repositories;
        public UnitOfWork(FiElSekkaContext context)
        {
            _context = context;
            _repositories = new Dictionary<string, object>();
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_context);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenericRepository<TEntity>;
        }
    }
}
