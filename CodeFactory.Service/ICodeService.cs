using CodeFactory.Core.Model;
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

        Task<CodePath> GetPathTree(CodePath path);

        Task<bool> CreateCodeInfo(CodeInfo info);

        Task<bool> UpdateCodeInfo(CodeInfo info);

        Task<CodeInfo> GetCodeInfo(CodeInfo info);

        Task<bool> DeleteCodeInfo(CodeInfo info);

        Task<List<CodeInfo>> GetCodeInfos(Guid pathId);
    }
}
