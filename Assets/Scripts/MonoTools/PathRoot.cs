#region License
/*
 * Copyright 2020 Autoware Foundation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Authors: AutoCore Members
 *
 */
#endregion


using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class PathRoot : MonoBehaviour
    {
        public string path;
        public GameObject pathPrefab;
        private DirectoryInfo root;
        private FileInfo[] files;
        private StreamReader sr;
        private Vector4 info;
        public List<Vector4> PathStartPoint;
        public bool isSet;
        public GameObject[] pathGo;
        private void OnEnable()
        {
            PathStartPoint = new List<Vector4> { };
            if (isSet)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    PathStartPoint.Add(transform.GetChild(i).GetComponent<PathInfo>().FirstPoint);
                }
            }
            else
            {
                pathPrefab = Resources.Load("Roads/Path") as GameObject;
                root = new DirectoryInfo(path);
                files = root.GetFiles();
                foreach (var file in files)
                {
                    if (file.Name.EndsWith(".txt"))
                    {
                        var infoList = new List<Vector4>();
                        string name = file.Name.Replace(".txt", "");
                        sr = new StreamReader(file.FullName);
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] strInfo = line.Split(',');
                            if (strInfo.Length == 4)
                            {
                                float.TryParse(strInfo[0], out info.x);
                                float.TryParse(strInfo[1], out info.y);
                                float.TryParse(strInfo[2], out info.z);
                                float.TryParse(strInfo[3], out info.w);
                                infoList.Add(info);
                            }
                            else
                            {
                            }
                        }
                        sr.Close();
                        AddPath(infoList, name);
                    }
                }
            }
        }

        public GameObject pointPrefab;
        private int index;
        private void AddPath(List<Vector4> iList, string name)
        {
            List<Vector4> tempIList = iList;
            GameObject objPath = Instantiate(pathPrefab);
            objPath.transform.position = new Vector3(tempIList[0].x, tempIList[0].y, tempIList[0].z);
            objPath.transform.SetParent(transform);
            objPath.name = "path-" + name;
            var o = objPath.GetComponent<PathInfo>();
            if (o)
            {
                o.arrInfo = tempIList.ToArray();
                o.FirstPoint = tempIList[0];
                o.LastPoint = tempIList[tempIList.Count - 1];
                PathStartPoint.Add(tempIList[0]);
            }
            if (!pointPrefab) pointPrefab = Resources.Load("Roads/PathPoint") as GameObject;
            index = 0;
            foreach (Vector4 item in tempIList)
            {
                GameObject point = Instantiate(pointPrefab);
                point.transform.position = new Vector3(item.x, item.y, item.z);
                point.transform.localScale = new Vector3(item.w, 0, 0);
                //point.transform.localScale = new Vector3(1, 1, 1);
                point.name = "PathPoint" + index.ToString();
                point.transform.SetParent(objPath.transform);
                index++;
            }
        }
    }

}