using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository.Interfaces;
using Data.Repository.Implementations.Repositories.Security;
using Data.Repository.Implementations.Repositories.Multibanca;

namespace Data.Repository.Implementations
{
    public class MultibancaUnitOfWork :IMultibancaUnitOfWork
    {
        private readonly MultibancaDBContext _dbContext;
        private bool _disposed;

        public LoginRepository LoginRepositoryProvider { get; }
        public RoleRepository RoleRepositoryProvider { get; }

        public MultibancaUnitOfWork(MultibancaDBContext skayrosDBContext)
        {
            _dbContext = skayrosDBContext;
            LoginRepositoryProvider = new LoginRepository(_dbContext);
            RoleRepositoryProvider = new RoleRepository(_dbContext);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            _disposed = true;
        }

        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
