#region Apache License, Version 2.0
//
// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
//
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace NMaven.Artifact
{
    public sealed class ArtifactRepository
    {
        public Artifact GetArtifactFor(String uri)
        {
            DirectoryInfo uac = new DirectoryInfo(localRepository.FullName + @"\uac\gac_msil\");

            String[] tokens = uri.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int size = tokens.Length;
            if (size < 3)
            {
                throw new IOException("Invalid Artifact Path = " + uri);
            }
            Artifact artifact = new Artifact();
            artifact.ArtifactId = tokens[size - 3];
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < size - 3; i++)
            {
                buffer.Append(tokens[i]);
                if (i != size - 4)
                {
                    buffer.Append(".");
                }
            }
            artifact.GroupId = buffer.ToString();
            artifact.Version = tokens[size - 2];
            artifact.Extension = tokens[size - 1].Split(".".ToCharArray())[1];
            artifact.FileInfo = new FileInfo(uac.FullName + artifact.ArtifactId + @"\" +
                artifact.Version + "__" + artifact.GroupId + @"\" + artifact.ArtifactId + ".dll" );
            return artifact;
        }

        public List<Artifact> GetArtifacts()
        {
            List<Artifact> artifacts = new List<Artifact>();
            DirectoryInfo uac = new DirectoryInfo(localRepository.FullName + @"\uac\gac_msil\");
            int directoryStartPosition = uac.FullName.Length;

            List<FileInfo> fileInfos = GetArtifactsFromDirectory(uac);

            foreach (FileInfo fileInfo in fileInfos)
            {
                //Artifact artifact = new Artifact();



                //String relativePath = fileInfo.FullName.Substring(directoryStartPosition,
                //    (fileInfo.FullName.Length - directoryStartPosition));
                //String[] tokens = relativePath.Split(new char[1] { '\\' });
                //artifact.ArtifactId = tokens[0];
                //String[] tokens2 = tokens[1].Split(new char[2]{'_', '_'});
                //artifact.Version = tokens2[0];
                //artifact.GroupId = tokens2[2];
                //artifact.FileInfo = fileInfo;

                try
                {
                    Artifact artifact = getArtifact(uac,fileInfo);
                    artifacts.Add(artifact);
                }
                catch
                {
                    
                }
            }
            return artifacts;
        }

        #region Repository Artifact Info Helper

       



        Artifact getArtifact(DirectoryInfo uacDirectory, FileInfo artifactFile)
        {
            string[] tokens = getRelativePathTokens(uacDirectory, artifactFile);
            

            // first file token is the artifact
            // eg. NMaven.VisualStudio.Addin\1.1.1.1__NMaven.VisualStudio\NMaven.VisualStudio.Addin.dll
            string artifactId = tokens[0];
            string groupId = getGroupId(tokens[1]);
            string version = getVersion(tokens[1]);


            Artifact artifact = new Artifact();
            artifact.ArtifactId = artifactId;
            artifact.Version = version;
            artifact.GroupId = groupId;
            artifact.FileInfo = artifactFile;
            return artifact;
        }



        string getVersion(string versionAndGroupDirectoryName)
        {
            // 1.1.1.1__NMaven.VisualStudio from index 0 to __ is the version number
            int index = versionAndGroupDirectoryName.IndexOf("__", 0);
            string str = versionAndGroupDirectoryName.Substring(0, index);
            return str;
        }

        string getGroupId(string versionAndGroupDirectoryName)
        {
            int index = versionAndGroupDirectoryName.IndexOf("__", 0) + 2;
            // 1.1.1.1__NMaven.VisualStudio from (next to index of __) to last is the groupid
            string str = versionAndGroupDirectoryName.Substring(index, (versionAndGroupDirectoryName.Length - index));
            return str;

        }

        #endregion

        #region Path Helper Methods


        string[] getRelativePathTokens(DirectoryInfo parentPath, FileInfo path)
        {
            return getRelativePathTokens(parentPath.FullName, path.FullName);
        }

        string[] getRelativePathTokens(DirectoryInfo parentPath, DirectoryInfo path)
        {
            return getRelativePathTokens(parentPath.FullName, path.FullName);
        }

        string[] getRelativePathTokens(FileInfo parentPath, FileInfo path)
        {
            return getRelativePathTokens(parentPath.FullName, path.FullName);
        }


        string[] getRelativePathTokens(string parentPath, string path)
        {
            string[] parent = tokenizePath(parentPath);
            string[] child = tokenizePath(path);

            List<string> list = new List<string>();
            
            list.AddRange(child);
            list.RemoveRange(0, parent.Length-1);
            
            return list.ToArray();

        }


        string[] tokenizePath(FileInfo fileInfo)
        {
            return tokenizePath(fileInfo.FullName);
        }

        string[] tokenizePath(DirectoryInfo directoryInfo)
        {
            return tokenizePath(directoryInfo.FullName);
        }

        string[] tokenizePath(string filename)
        {
            string path = Path.GetFullPath(filename);
            path = path.Replace('/', '\\');

            return path.Split('\\');

        }

        #endregion

        public void Init(ArtifactContext artifactContext, DirectoryInfo localRepository)
        {
            this.artifactContext = artifactContext;
            this.localRepository = localRepository;
        }

        private List<FileInfo> GetArtifactsFromDirectory(DirectoryInfo baseDirectoryInfo)
        {
            DirectoryInfo[] directories = baseDirectoryInfo.GetDirectories();
            List<FileInfo> fileInfos = new List<FileInfo>();
            foreach (DirectoryInfo directoryInfo in directories)
            {
                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    if (fileInfo.Name.EndsWith(".dll") || fileInfo.Name.EndsWith(".exe") || fileInfo.Name.EndsWith(".netmodule") )
                    {
                        fileInfos.Add(fileInfo);
                    }              
                }
                fileInfos.AddRange(GetArtifactsFromDirectory(directoryInfo));
            }
            return fileInfos;
        }

        private ArtifactContext artifactContext;
        private DirectoryInfo localRepository;
    }
}
