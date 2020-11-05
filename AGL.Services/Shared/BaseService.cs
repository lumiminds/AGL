using System;
using AGL.Models;
using AutoMapper;

namespace AGL.Services.Shared
{
    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        private readonly IMapper _mapper;
        public IMapper Mapper { get { return _mapper; } }

        protected BaseService(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
