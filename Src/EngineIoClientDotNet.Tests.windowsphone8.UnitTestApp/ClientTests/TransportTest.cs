﻿//using log4net;

using EngineIoClientDotNet.Modules;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quobject.EngineIoClientDotNet.Client;
using Quobject.EngineIoClientDotNet.Client.Transports;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Quobject.EngineIoClientDotNet_Tests.ClientTests
{
    // NOTE: tests for the rememberUpgrade option are on ServerConnectionTest.

    [TestClass]    
    public class TransportTest : Connection
    {
        [TestMethod]
        public void Constructors()
        {
            LogManager.SetupLogManager();
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod());
            log.Info("Start");

            var socket = new Socket(CreateOptions());

            socket.Open();
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.IsNotNull(socket.Transport);

            socket.Close();
        }

        [TestMethod]
        public void Uri()
        {
            LogManager.SetupLogManager();
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod());
            log.Info("Start");

            var options = new Transport.Options();
            options.Path = "/engine.io";
            options.Hostname = "localhost";
            options.Secure = false;
            options.Query = new Dictionary<string, string> {{"sid", "test"}};
            options.TimestampRequests = false;
            var polling = new Polling(options);
            Assert.IsTrue(polling.Uri().StartsWith("http://localhost/engine.io?sid=test&b64=1"));
        }

        [TestMethod]
        public void UriWithDefaultPort()
        {
            LogManager.SetupLogManager();
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod());
            log.Info("Start");


            var options = new Transport.Options();
            options.Path = "/engine.io";
            options.Hostname = "localhost";
            options.Secure = false;
            options.Query = new Dictionary<string, string> {{"sid", "test"}};
            options.TimestampRequests = false;
            options.Port = 80;
            var polling = new Polling(options);
            Assert.IsTrue(polling.Uri().StartsWith("http://localhost/engine.io?sid=test&b64=1"));
        }

        [TestMethod]
        public void UriWithPort()
        {
            LogManager.SetupLogManager();
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod());
            log.Info("Start");


            var options = new Transport.Options();
            options.Path = "/engine.io";
            options.Hostname = "localhost";
            options.Secure = false;
            options.Query = new Dictionary<string, string> {{"sid", "test"}};
            options.TimestampRequests = false;
            options.Port = 3000;
            var polling = new Polling(options);            
            Assert.IsTrue(polling.Uri().StartsWith("http://localhost:3000/engine.io?sid=test&b64=1"));
        }


        [TestMethod]
        public void HttpsUriWithDefaultPort()
        {
            LogManager.SetupLogManager();
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod());
            log.Info("Start");

            var options = new Transport.Options();
            options.Path = "/engine.io";
            options.Hostname = "localhost";
            options.Secure = true;
            options.Query = new Dictionary<string, string> {{"sid", "test"}};
            options.TimestampRequests = false;
            options.Port = 443;
            var polling = new Polling(options);
            //Assert.Contains("https://localhost/engine.io?sid=test&b64=1", polling.Uri());
            Assert.IsTrue(polling.Uri().StartsWith("https://localhost/engine.io?sid=test&b64=1"));

        }


        [TestMethod]
        public void TimestampedUri()
        {
            LogManager.SetupLogManager();
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod());
            log.Info("Start");

            var options = new Transport.Options();
            options.Path = "/engine.io";
            options.Hostname = "localhost";
            options.Secure = false;
            options.Query = new Dictionary<string, string> {{"sid", "test"}};
            options.TimestampRequests = true;
            options.TimestampParam = "t";
            var polling = new Polling(options);

            string pat = @"http://localhost/engine.io\?sid=test&(t=[0-9]+-[0-9]+)";
            var r = new Regex(pat, RegexOptions.IgnoreCase);
            var test = polling.Uri();
            log.Info(test);
            Match m = r.Match(test);
            Assert.IsNull(m.Success);
        }


        [TestMethod]
        public void WsUri()
        {
            LogManager.SetupLogManager();
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod());
            log.Info("Start");

            var options = new Transport.Options();
            options.Path = "/engine.io";
            options.Hostname = "test";
            options.Secure = false;
            options.Query = new Dictionary<string, string> {{"transport", "websocket"}};
            options.TimestampRequests = false;
            var ws = new WebSocket(options);
            //Assert.Contains("ws://test/engine.io?transport=websocket", ws.Uri());
            Assert.IsTrue(ws.Uri().StartsWith("ws://test/engine.io?transport=websocket"));
        }

        [TestMethod]
        public void WssUri()
        {
            LogManager.SetupLogManager();
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod());
            log.Info("Start");

            var options = new Transport.Options();
            options.Path = "/engine.io";
            options.Hostname = "test";
            options.Secure = true;
            options.Query = new Dictionary<string, string> {{"transport", "websocket"}};
            options.TimestampRequests = false;
            var ws = new WebSocket(options);
            //Assert.Contains("wss://test/engine.io?transport=websocket", ws.Uri());
            Assert.IsTrue(ws.Uri().StartsWith("wss://test/engine.io?transport=websocket"));
        }


        [TestMethod]
        public void WsTimestampedUri()
        {
            LogManager.SetupLogManager();
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod());
            log.Info("Start");


            var options = new Transport.Options();
            options.Path = "/engine.io";
            options.Hostname = "localhost";
            options.Secure = false;
            options.Query = new Dictionary<string, string> {{"sid", "test"}};
            options.TimestampRequests = true;
            options.TimestampParam = "woot";
            var ws = new WebSocket(options);

            string pat = @"ws://localhost/engine.io\?sid=test&(woot=[0-9]+-[0-9]+)";
            var r = new Regex(pat, RegexOptions.IgnoreCase);
            var test = ws.Uri();
            log.Info(test);
            Match m = r.Match(test);
            Assert.IsNull(m.Success);
        }

    }
}
