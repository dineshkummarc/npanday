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
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 1.1.4322.2032
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

// 
//This source code was auto-generated by MonoXSD
//
namespace NPanday.Plugin.Settings {
    
    
    /// <remarks/>
    public class npandaySettings {
        
        /// <remarks/>
        public string operatingSystem;
        
        /// <remarks/>
        public string architecture;
        
        /// <remarks/>
        public npandaySettingsDefaultSetup defaultSetup;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItem(ElementName="vendor", IsNullable=false)]
        public npandaySettingsVendorsVendor[] vendors;
    }
}
