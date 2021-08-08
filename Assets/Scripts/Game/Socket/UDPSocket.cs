using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class UDPSocket : MonoBehaviour
{
    Socket udpSocket;

    public UDPSocket(int port)
    {
        Initial(port);
    }

    public void Initial(int port)
    {
        //AddressFamily---->地址协议族:Ipv4或Ipv6
        //SocketType------->数据传递方式,与协议相关:UDP则为数据报(Dgram);TCP则为数据流(Stream)
        //ProtocolType----->协议:UDP或TCP  
        //初始话一个UDP型的Socket
        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        

        //IPAddress-------->网卡地址:IPAddress.Any表示本机网卡地址
        //port------------->端口号
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, port);
        //绑定IP地址和端口号
        udpSocket.Bind(ipPoint);
    }
}