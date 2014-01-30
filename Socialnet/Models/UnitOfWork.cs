using Socialnet.Models.Repositories;
using System;

namespace Socialnet.Models
{
    public class UnitOfWork : IDisposable
    {
        private SocialContext context = new SocialContext();
        private bool disposed = false;
        private NewsFeedEngine newsFeedEngine;
        private ShareRepository sharesRepository;

        public SocialContext Context
        {
            get
            {
                return context;
            }
        }

        public NewsFeedEngine NewsFeedEngine
        {
            get
            {
                if (newsFeedEngine == null)
                    newsFeedEngine = new NewsFeedEngine(context);
                return newsFeedEngine;
            }
        }

        public ShareRepository SharesRepository
        {
            get
            {
                if (this.sharesRepository == null)
                    this.sharesRepository = new ShareRepository(context);
                return this.sharesRepository;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}