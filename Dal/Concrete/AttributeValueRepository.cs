using Core.Entity;
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
    public class AttributeValueRepository : EfRepository<AttributeValue>, IAttributeValueRepository
    {
        private BoynerContext _ctx;
        public AttributeValueRepository(BoynerContext context) : base(context)
        {
            _ctx = context;
        }
    }
}
