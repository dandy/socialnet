using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Socialnet.Models.Repositories
{
    public class ShareRepository : IDisposable
    {
        private SocialContext context;

        public ShareRepository(SocialContext context)
        {
            this.context = context;
        }

        public void AddShare(Share obj){
            context.Shares.Add(obj);
        }

        public Share GetShare(int shareId)
        {
            return context.Shares.Find(shareId);
        }

        public IEnumerable<Share> GetAllShares()
        {
            return context.Shares.ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

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

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
    }
}