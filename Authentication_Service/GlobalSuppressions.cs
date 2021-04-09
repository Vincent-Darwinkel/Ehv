// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S4457:Parameter validation in \"async\"/\"await\" methods should be wrapped", Justification = "<Pending>", Scope = "member", Target = "~M:Authentication_Service.Logic.JwtLogic.CreateJwt(Authentication_Service.Models.Dto.UserDto,Authentication_Service.Models.Dto.RefreshTokenDto)~System.Threading.Tasks.Task{Authentication_Service.Models.ToFrontend.LoginResultViewmodel}")]
