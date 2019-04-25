using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net;

public class TitleController : MonoBehaviour
{
    string fileNam = "/save.dt";
    Save saveData;
    BinaryFormatter bf = new BinaryFormatter();

    // Start is called before the first frame update
    void Start()
    {
        Game.instance = new Game();
        Screen.SetResolution(1080,1920,false);
		PanelMgr.instance.OpenPanel<TitlePanel>("");

        //扫描缓存文件，如果没有设定过名字则打开名字设定面板
        //如果已经有名字了则查询服务器
        if(File.Exists(Application.persistentDataPath + fileNam))
        {
            FileStream f = null;
            try
            {
                f = File.Open(Application.persistentDataPath + fileNam, FileMode.Open);
                saveData = (Save)bf.Deserialize(f);
            }
            catch(IOException)
            {
            }
            catch(System.Runtime.Serialization.SerializationException)
            {
                saveData = null;
            }
            finally
            {
                f.Close();
            }
            
            if(saveData.username == "" || saveData.username == null)  //没有设置过名字
            {
                PanelMgr.instance.OpenPanel<SetNamePanel>("");
            }
        }
        else
        {
            //文件不存在，则设置名字并新建一个
            PanelMgr.instance.OpenPanel<SetNamePanel>("");
        }
        GetListFromServer();
    }

    public void GetListFromServer()
    {
        //将服务器排行榜数据弄下来
        byte[] result = new byte[1024];
        int bytes = 0;
        IPAddress ip = IPAddress.Parse(Game.instance.HOST);
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
        //更新自己的最高分数
        try
        {
            sock.Connect(new IPEndPoint(ip, Game.instance.PORT));
            sock.Send(System.Text.Encoding.Default.GetBytes("USER " + saveData.username + " " + saveData.maxScore.ToString()));
            bytes = sock.Receive(result);
        }
        catch
        {
            PanelMgr.instance.OpenPanel<TipsPanel>("", "服务器异常");
        }
        try
        {
            sock.Send(System.Text.Encoding.Default.GetBytes("GETTOP"));
            bytes = sock.Receive(result);
        }
        catch
        {
            PanelMgr.instance.OpenPanel<TipsPanel>("", "服务器异常");
        }
        if(bytes != 0)
        {
            string str = System.Text.Encoding.Default.GetString (result);
            saveData.SetList(str);
        }
        sock.Close();

        if(File.Exists(Application.persistentDataPath + fileNam))
        {
            FileStream f = null;
            try
            {
                f = File.Open(Application.persistentDataPath + fileNam, FileMode.Create);
                bf.Serialize(f, saveData);
            }
            catch(IOException)
            {
            }
            catch(System.Runtime.Serialization.SerializationException)
            {
            }
            finally
            {
                f.Close();
            }
        }
        return;
    }
}
