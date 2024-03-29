﻿using PIM.Api.Core.Models;

namespace PIM.Api.Infra.AuthEngine.Interface
{
    public interface ITokenProvider
    {
        string GenerateToken(string hash, SystemUser user);
    }
}
