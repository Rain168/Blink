﻿using Net.Qiujuer.Blink.Core;
using Net.Qiujuer.Blink.Listener;
using Net.Qiujuer.Blink.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Net.Qiujuer.Blink
{
    public class Blink
    {
        /**
     * Default on-disk resource directory.
     */
        private static readonly String DEFAULT_RESOURCE_DIR = "blink";
        /**
         * Default buffer size
         */
        private static readonly int DEFAULT_SOCKET_BUFFER_SIZE = 4 * 1024 * 1024;

        /**
         * Create a Bink connection by socket
         *
         * @param socket           Socket
         * @param socketBufferSize Socket BufferSize
         * @param resourcePath     File Resource Path
         * @param fileMark         File name mark to clear
         * @param listener         ReceiveListener
         * @return BlinkConn
         * @throws Exception
         */
        public static BlinkConn newConnection(Socket socket, int socketBufferSize, String resourcePath, String fileMark, BlinkListener listener)
        {
            String path = Path.Combine(resourcePath, DEFAULT_RESOURCE_DIR);
            DiskResource resource = new DiskResource(path, fileMark);
            BlinkParser parser = new BlinkParser(resource);
            SocketAdapter socketAdapter = new SocketAdapter(socket, socketBufferSize, parser);
            ExecutorDelivery delivery = new ExecutorDelivery(listener);
            return new BlinkConn(socketAdapter, delivery, socketAdapter, delivery, resource);
        }

        /**
         * Create a Bink connection by socket
         *
         * @param socket       Socket
         * @param resourcePath File Resource Path
         * @param fileMark     File name mark to clear
         * @param listener     ReceiveListener
         * @return BlinkConn
         * @throws Exception
         */
        public static BlinkConn newConnection(Socket socket, String resourcePath, String fileMark, Executor executor, BlinkListener listener)
        {
            return newConnection(socket, DEFAULT_SOCKET_BUFFER_SIZE, resourcePath, fileMark, listener);
        }

        /**
         * Create a Bink connection by socket
         *
         * @param socket           Socket
         * @param socketBufferSize Socket BufferSize
         * @param resourcePath     File Resource Path
         * @param listener         ReceiveListener
         * @return BlinkConn
         * @throws Exception
         */
        public static BlinkConn newConnection(Socket socket, int socketBufferSize, String resourcePath, Executor executor, BlinkListener listener)
        {
            return newConnection(socket, socketBufferSize, resourcePath, Guid.NewGuid().ToString(), listener);
        }

        /**
         * Create a Bink connection by socket
         *
         * @param socket       Socket
         * @param resourcePath File Resource Path
         * @param listener     ReceiveListener
         * @return BlinkConn
         * @throws Exception
         */
        public static BlinkConn newConnection(Socket socket, String resourcePath, BlinkListener listener)
        {
            return newConnection(socket, DEFAULT_SOCKET_BUFFER_SIZE, resourcePath, Guid.NewGuid().ToString(), listener);
        }

        /**
         * Create a Bink connection by socket
         *
         * @param socket   Socket
         * @param listener ReceiveListener
         * @return BlinkConn
         * @throws Exception
         */
        public static BlinkConn newConnection(Socket socket, BlinkListener listener)
        {
            return newConnection(socket, DEFAULT_SOCKET_BUFFER_SIZE, getDefaultResourcePath(), Guid.NewGuid().ToString(), listener);
        }

        class SingleExecutor : Executor
        {
            public void Execute(Runnable command)
            {
                command.Run();
            }
        }


        /**
         * Get Default path with CanonicalPath
         *
         * @return Path
         * @throws IOException
         */
        private static String getDefaultResourcePath()
        {
            return System.IO.Directory.GetCurrentDirectory();
        }
    }
}
