﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftSmrBot.Core;
using GiftSmrBot.Core.DataInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftSmrBot.Services.Data
{
    public class EntityService<TEntity, TKey> : IEntityService<TEntity, TKey> where TEntity : class, new()
    {
        protected internal ApplicationContext context;
        protected internal DbSet<TEntity> dbSet;

        public EntityService(ApplicationContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual async Task Create(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> Delete(TKey id)
        {
            var entity = await GetById(id);
            return await Delete(entity);
        }

        public async Task<bool> Delete(TEntity entity)
        {
            if (entity != null)
            {
                dbSet.Remove(entity);
                return await context.SaveChangesAsync() != -1;
            }
            return false;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet.AsNoTracking();
        }

        public async Task<TEntity> GetById(TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            //dbSet.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
