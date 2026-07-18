using Framework.WorkFlow.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WorkFlow.Repository.Implementation
{
    public class WorkFlowDBContext : DbContext, IWorkFlowDBContext
    {
        public WorkFlowDBContext(DbContextOptions<WorkFlowDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
