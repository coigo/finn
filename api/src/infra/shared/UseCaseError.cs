namespace Infra.Shared;

public class BusinessError: Exception {

    override public string Message { get; }

    public BusinessError (string message) {
        Message = message;
    } 

}