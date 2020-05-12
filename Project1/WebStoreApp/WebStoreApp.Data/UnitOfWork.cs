using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebStoreApp.Domain;
using WebStoreApp.Domain.Interfaces;
using WebStoreApp.Data.Repository;

namespace WebStoreApp.Data
{
    public class UnitOfWork : IDisposable
    {
        private WebStoreAppContext _context;

        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        private IProductRepository _productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (this._productRepository == null)
                {
                    this._productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }

        private ILocationRepository _locationRepository;
        public ILocationRepository LocationRepository
        {
            get
            {
                if (this._locationRepository == null)
                {
                    this._locationRepository = new LocationRepository(_context);
                }
                return _locationRepository;
            }
        }

        private IOrderRepository _orderRepository;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (this._orderRepository == null)
                {
                    this._orderRepository = new OrderRepository(_context);
                }
                return _orderRepository;
            }
        }

        public UnitOfWork (WebStoreAppContext context)
        {
            this._context = context;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        #region IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}