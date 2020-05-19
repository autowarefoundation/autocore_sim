#region License
/*
* Copyright 2018 AutoCore
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
*/
#endregion


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T:class,new()
{
    protected static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
    protected Singleton()
    {
        if (_instance != null)
        {
            Debug.LogError("this"+(typeof(T)).ToString()+"is not null");
        }
        Init();
    }
    public virtual void Init()
    {

    }
}
