using System;
using System.ServiceModel;

namespace GettingStartedLib
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IFridge
    {
        [OperationContract]
        int Add(string fruit, int count);
        [OperationContract]
        int Subtract(string fruit, int count);
        [OperationContract]
        int Get(string fruit);
    }
}