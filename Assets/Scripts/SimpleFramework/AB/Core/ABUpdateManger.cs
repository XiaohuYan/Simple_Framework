using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace SimpleFramework.AB
{
    public class ABUpdateManager : IABUpdateManager
    {
        private int priority = 0;

        public int Priority => priority;

        /// <summary>
        /// 用于存储云端的 AB 包信息
        /// </summary>
        private readonly Dictionary<string, ABInfo> remoteABInfo = new Dictionary<string, ABInfo>();

        /// <summary>
        /// 下载列表
        /// </summary>
        private readonly Queue<string> downLoadList = new Queue<string>();

        /// <summary>
        /// 用于存储本地的 AB 包信息
        /// </summary>
        private readonly Dictionary<string, ABInfo> localABInfo = new Dictionary<string, ABInfo>();

        /// <summary>
        /// 下载地址
        /// </summary>
        private readonly string downLoadPath = Application.streamingAssetsPath + "/";

        /// <summary>
        /// 下载入口
        /// </summary>
        /// <param name="overCallBack">下载完成调用</param>
        public async void CheckUpdate(UnityAction<bool> overCallBack)
        {
            downLoadList.Clear();
            localABInfo.Clear();
            remoteABInfo.Clear();

            DownLoadRemoteABCompareFile();
            Debug.Log("解析远端对比文件完成");
            await GetLocalABCompareFileInfo();
            Debug.Log("解析本地对比文件完成");
            foreach (var name in remoteABInfo.Keys)
            {
                if (!localABInfo.ContainsKey(name))
                {
                    downLoadList.Enqueue(name);
                }
                else
                {
                    if (localABInfo[name].md5 != remoteABInfo[name].md5)
                    {
                        downLoadList.Enqueue(name);
                        // 移除本地信息，那么剩下的就是本地有远端没有的信息
                        localABInfo.Remove(name);
                    }
                }
            }

            // 删除远端没有的 ab 文件
            foreach (var name in localABInfo.Keys)
            {
                if (File.Exists(Application.persistentDataPath + "/" + name))
                {
                    File.Delete(Application.persistentDataPath + "/" + name);
                }
            }

            DownLoadABFile();

            File.WriteAllText(Application.persistentDataPath + "/ABCompareInfo.txt", File.ReadAllText(downLoadPath + "ABCompareInfo_TMP.txt"));
        }

        /// <summary>
        /// 下载远端的 AB 包信息文件
        /// </summary>
        private void DownLoadRemoteABCompareFile()
        {
            // 手机使用 Application.persistentDataPath
            DownLoadFile("ABCompareInfo.txt", downLoadPath + "ABCompareInfo_TMP.txt");
            // 读取对比文件
            string info = File.ReadAllText(downLoadPath + "ABCompareInfo_TMP.txt");
            DeSerialABCompareInfo(info, remoteABInfo);
        }

        /// <summary>
        /// 解析 AB 包文件信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ABInfo"></param>
        private void DeSerialABCompareInfo(string info, Dictionary<string, ABInfo> ABInfo)
        {
            string[] strs = info.Split('|');
            string[] infos = null;
            for (int i = 0; i < strs.Length; i++)
            {
                infos = strs[i].Split(' ');
                ABInfo abInfo = new ABInfo(infos[0], infos[1], infos[2]);
                ABInfo.Add(infos[0], abInfo);
            }
        }


        /// <summary>
        /// 从远端下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="localPath"></param>
        private void DownLoadFile(string fileName, string localPath)
        {
            // 创建啊一个FTP连接
            FtpWebRequest rep = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/AB/" + fileName)) as FtpWebRequest;
            // 设置通信凭证(如果有匿名账号 可以不设置凭证)
            NetworkCredential n = new NetworkCredential("YanXiaohu", "yan199821hu");
            rep.Credentials = n;
            // 设置代理
            rep.Proxy = null;
            // 请求完毕后是否关闭控制连接
            rep.KeepAlive = false;
            // 操作命令上传文件
            rep.Method = WebRequestMethods.Ftp.DownloadFile;
            // 指定传输的类型 2进制
            rep.UseBinary = true;
            // 下载
            FtpWebResponse res = rep.GetResponse() as FtpWebResponse;
            using (Stream downLoadStream = res.GetResponseStream())
            {
                using (FileStream file = File.Create(localPath))
                {
                    // 一点一点的下载内容
                    byte[] bytes = new byte[2048];
                   // 返回值代表读取了多少个字节
                    int count = downLoadStream.Read(bytes, 0, bytes.Length);

                    // 循环下载数据
                    while (count != 0)
                    {
                        file.Write(bytes, 0, count);
                        count = downLoadStream.Read(bytes, 0, bytes.Length);
                    }
                } 
            }
            Debug.Log(fileName + "下载成功");
        }

        /// <summary>
        /// 下载远端的 AB 包
        /// </summary>
        private async void DownLoadABFile()
        {
            foreach (var name in remoteABInfo.Keys)
            {
                downLoadList.Enqueue(name);
            }
            await Task.Run(() =>
            {
                while (downLoadList.Count > 0)
                {
                    DownLoadFile(downLoadList.Peek(), downLoadPath + downLoadList.Peek());
                    downLoadList.Dequeue();
                }
            });
        }

        /// <summary>
        /// 获取本地的 AB 信息文件
        /// </summary>
        private async Task GetLocalABCompareFileInfo()
        {
            if (File.Exists(Application.persistentDataPath + "/ABCompareInfo.txt"))
            {
                await GetLocalABCompareFileInfo(Application.persistentDataPath + "/ABCompareInfo.txt");
            }
            else if (File.Exists(Application.streamingAssetsPath + "/ABCompareInfo.txt"))
            {
                await GetLocalABCompareFileInfo(Application.streamingAssetsPath + "/ABCompareInfo.txt");
            }
        }

        /// <summary>
        /// 获取本地的 AB 信息文件的协程
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task GetLocalABCompareFileInfo(string filePath)
        {
            // 使用 unityWebRequest 读取文件可以防止跨平台问题
            UnityWebRequest req = UnityWebRequest.Get(filePath);
            req.SendWebRequest();
            while (!req.isDone)
            {
                await Task.Yield();
            }
            DeSerialABCompareInfo(req.downloadHandler.text, localABInfo);
        }


        public void AfterManagerInit()
        {
            
        }

        public void OnManagerInit()
        {
            
        }

        public void OnManagerDestroy()
        {
            remoteABInfo.Clear();
            downLoadList.Clear();
            localABInfo.Clear();
        }
    }
}