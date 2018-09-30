using System;
using System.Net;
using UnityEngine;
using Matrix;
using Matrix.License;
using Matrix.Xmpp.Client;
using Matrix.Xmpp.Sasl;

public class Messenger
{
    #region Singleton

    private static Messenger instance = null;

    public static Messenger Instance
    {
        get
        {
            if (instance == null)
            {
                return instance = new Messenger ();
            }

            return instance;
        }
    }

    private Messenger ()
    {
        LicenseManager.SetLicense (Constants.LICENSE);

        RegisterEvents ();
    }

    #endregion

    private XmppClient xmppClient = new XmppClient ();

    private void RegisterEvents ()
    {
        xmppClient = new XmppClient ();

        xmppClient.OnReceiveXml += new EventHandler<TextEventArgs> (XmppClientOnReceiveXml);

        xmppClient.OnSendXml += new EventHandler<TextEventArgs> (XmppClientOnSendXml);

        xmppClient.OnLogin += new EventHandler<Matrix.EventArgs> (OnLogin);

        xmppClient.OnAuthError += new EventHandler<SaslEventArgs> (OnAuthError);

        xmppClient.OnBeforeSasl += new EventHandler<SaslEventArgs> (OnBeforeSasl);

        xmppClient.OnError += new EventHandler<ExceptionEventArgs> (OnError);

        /*
        < Messenger > OnError : (System.IO.IOException: The authentication or decryption has failed. --->System.IO.IOException: The authentication or decryption has failed. --->Mono.Security.Protocol.Tls.TlsException: The authentication or decryption has failed.
        at Mono.Security.Protocol.Tls.RecordProtocol.EndReceiveRecord (System.IAsyncResult asyncResult) [0x00037] in < 59f5789d548a4d9d86fbc012db4951c0 >:0
        at Mono.Security.Protocol.Tls.SslClientStream.SafeEndReceiveRecord (System.IAsyncResult ar, System.Boolean ignoreEmpty) [0x00000] in < 59f5789d548a4d9d86fbc012db4951c0 >:0
        at Mono.Security.Protocol.Tls.SslClientStream.NegotiateAsyncWorker (System.IAsyncResult result) [0x00071] in < 59f5789d548a4d9d86fbc012db4951c0 >:0
        
        -- - End of inner exception stack trace-- -
        
        at Mono.Security.Protocol.Tls.SslClientStream.EndNegotiateHandshake (System.IAsyncResult result) [0x00032] in < 59f5789d548a4d9d86fbc012db4951c0 >:0
        at Mono.Security.Protocol.Tls.SslStreamBase.AsyncHandshakeCallback (System.IAsyncResult asyncResult) [0x0000c] in < 59f5789d548a4d9d86fbc012db4951c0 >:0
        
        -- - End of inner exception stack trace-- -
        
        at Mono.Security.Protocol.Tls.SslStreamBase.EndRead (System.IAsyncResult asyncResult) [0x0004b] in < 59f5789d548a4d9d86fbc012db4951c0 >:0
        at Mono.Net.Security.Private.LegacySslStream.EndAuthenticateAsClient (System.IAsyncResult asyncResult) [0x0000e] in < 4b9f316768174388be8ae5baf2e6cc02 >:0
        at Mono.Net.Security.Private.LegacySslStream.AuthenticateAsClient (System.String targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates, System.Security.Authentication.SslProtocols enabledSslProtocols, System.Boolean checkCertificateRevocation) [0x0000e] in < 4b9f316768174388be8ae5baf2e6cc02 >:0
        at System.Net.Security.SslStream.AuthenticateAsClient (System.String targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates, System.Security.Authentication.SslProtocols enabledSslProtocols, System.Boolean checkCertificateRevocation) [0x00006] in < 4b9f316768174388be8ae5baf2e6cc02 >:0
        at Matrix.Net.ClientSocket.DoStartSecurityLayer (System.Security.Authentication.SslProtocols protocol) [0x00049] in < ad9fcdd9487e456c9eb900c33c80c918 >:0 )
        
        UnityEngine.Debug:LogFormat (String, Object [])
        Messenger:
        OnError (Object, ExceptionEventArgs) (at Assets / Scripts / Messenger.cs:105)
        
        System.Delegate:DynamicInvoke (Object [])
        Matrix.XmppStream:DoRaiseEvent (Delegate, Object [])
        Matrix.XmppStream:DoRaiseEvent (Delegate, Object, EventArgs)
        Matrix.XmppStream:socket_OnError (Object, ExceptionEventArgs)
        Matrix.Net.BaseSocket:FireOnError (Exception)
        Matrix.Net.ClientSocket:StartTls ()
        Matrix.Xmpp.Client.XmppClient:ProcessTls ()
        Matrix.Xmpp.Client.XmppClient:XmppStreamParser_OnStreamElement (Object, StanzaEventArgs)
        Matrix.Xml.XmppStreamParser:DoRaiseOnStreamElement (XmppXElement)
        Matrix.Xml.XmppStreamParser:EndTag (Byte [], Int32, ContentToken, TOK)
        Matrix.Xml.XmppStreamParser:Write (Byte [], Int32, Int32)
        Matrix.XmppStream:socket_OnReceive (Object, SocketEventArgs)
        Matrix.Net.BaseSocket:FireOnReceive (Byte [], Int32, Int32)
        Matrix.Net.ClientSocket:EndReceive (IAsyncResult)
        System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()
        */

        // ==============================================================================================================================
        // Ajay Birla - I'm trying below code to FIX above Exception but it is not working.
        // ==============================================================================================================================
        ServicePointManager.ServerCertificateValidationCallback +=
        delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
        System.Security.Cryptography.X509Certificates.X509Chain chain,
        System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback (delegate
        {
            return true;
        });
        // ==============================================================================================================================
    }

    private void XmppClientOnSendXml (object sender, TextEventArgs e)
    {
        AddDebug ("Sent : " + e.Text);
    }

    private void XmppClientOnReceiveXml (object sender, TextEventArgs e)
    {
        AddDebug ("Received : " + e.Text);
    }

    private void AddDebug (string debug)
    {
        Debug.Log (debug);
    }

    private void OnLogin (object sender, Matrix.EventArgs e)
    {
        Debug.LogFormat ("<Messenger> OnLogin : ({0})", e.ToString ());
    }

    private void OnAuthError (object sender, SaslEventArgs e)
    {
        Debug.LogErrorFormat ("<Messenger> OnAuthError : ({0})", e.ToString ());
    }

    private void OnBeforeSasl (object sender, SaslEventArgs e)
    {
        Debug.LogFormat ("<Messenger> OnBeforeSasl : ({0})", e.ToString ());
    }

    private void OnError (object sender, ExceptionEventArgs e)
    {
        Debug.LogErrorFormat ("<Messenger> OnError : ({0})", e.Exception.ToString ());
    }

    public void Login (string username, string password)
    {
        xmppClient.SetUsername (username);
        xmppClient.SetXmppDomain (Constants.DOMAIN);
        xmppClient.Password = password;
        xmppClient.Status = "I'm Chatty";
        xmppClient.Show = Matrix.Xmpp.Show.Chat;

        xmppClient.Open ();
    }
}