using Core.Repository;
using Dal.Abstract;
using DataAccess.Concrete.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Concrete.EF
{
    public class AttributeRepository : EfRepository<Core.Entity.Attribute>, IAttributeRepository
    {
        private BoynerContext _ctx;
        public AttributeRepository(BoynerContext context) : base(context)
        {
            _ctx = context;
        }
    }
}
