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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadMatch : MonoBehaviour
{
    public Transform begin;
    public Transform end;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = begin.position / 2 + end.position / 2;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, end.position - begin.position);
        transform.localScale = new Vector3(1, 1, Vector3.Distance(end.position, begin.position));
    }
}
