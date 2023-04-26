using System.Runtime.Serialization;

namespace CineClubApi.Common.ServiceResults;

public class ServiceResult : ISerializable
{
    public string Result { get; set; }
    public int StatusCode { get; set; }
    
    
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}