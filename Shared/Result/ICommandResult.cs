using System.Text.Json.Serialization;

namespace TechChallenge_auth_function.Shared.Result
{
    public interface ICommandResult
    {
        List<string> Messages { get; set; }

        [JsonIgnore]
        bool Succeeded { get; set; }

        [JsonIgnore]
        bool Conflict { get; set; }
    }

    public interface ICommandResult<T> : ICommandResult
    {
        T Data { get; set; }
    }
}
