using CodeFactory.Core.Model;
using CodeFactory.Service.Dto;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeFactory.Service
{
    [DynamicWebApi]
    public interface ICodeService : IDynamicWebApi
    {
        Task<bool> CreateCodePath(CodePath path);

        Task<bool> UpdateCodePath(CodePath path);

        Task<bool> DeleteCodePath(CodePath path);

        Task<GetPathTreeDto> GetPathTree(CodePath path);

        Task<bool> CreateCodeInfo(CodeInfo info);

        Task<bool> UpdateCodeInfo(CodeInfo info);

        Task<CodeInfo> GetCodeInfo(CodeInfo info);

        Task<bool> DeleteCodeInfo(CodeInfo info);

        Task<List<CodeInfo>> GetCodeInfos(Guid pathId);
    }
}
