using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace SDKLibrary
{
    public class TcpClientState
    {
        public TcpClientState(TcpClient tcpClient)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("null tcpClient");

            this.TcpClient = tcpClient;
            int receiveBufferSize = 9 * 1024;
            this.Buffer = new byte[receiveBufferSize];
            Offset = 0;
        }

        public TcpClientState(TcpClient tcpClient, byte[] buffer)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("null tcpClient");
            if (buffer == null)
                throw new ArgumentNullException("null buffer");

            this.TcpClient = tcpClient;
            this.Buffer = buffer;
            Offset = 0;
        }
        /// <summary>
        /// 接收数据长度
        /// </summary>
        public int RecvedLength { get; set; }
        

        /// <summary>
        /// 与客户端相关的TcpClient
        /// </summary>
        public TcpClient TcpClient { get; private set; }

        /// <summary>
        /// 获取缓冲区
        /// </summary>
        public byte[] Buffer { get; private set; }

        public int Offset { get; set; }


        public MemoryStream MemBufStream = new MemoryStream();

        /// <summary>
        /// 获取网络流
        /// </summary>
        public NetworkStream NetworkStream
        {
            get { return TcpClient.GetStream(); }
        }


        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            //关闭数据的接受和发送
            TcpClient.Close();
            Buffer = null;
            Offset = 0;
        }
    }
}
