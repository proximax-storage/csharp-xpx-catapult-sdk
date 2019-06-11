// Copyright 2019 ProximaX
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace ProximaX.Sirius.Sdk.Model.Namespaces
{
    /// <summary>
    ///     Class NamespaceName
    /// </summary>
    public class NamespaceName
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NamespaceName" /> class.
        /// </summary>
        /// <param name="namespaceId">The namespace Id</param>
        /// <param name="name">The namespace name</param>
        /// <param name="parentId">The namespace parent id</param>
        public NamespaceName(NamespaceId namespaceId, string name, NamespaceId parentId)
        {
            NamespaceId = namespaceId;
            Name = name;
            ParentId = parentId;
        }

        /// <summary>
        ///     The namespace Id
        /// </summary>
        public NamespaceId NamespaceId { get; }

        /// <summary>
        ///     The namespace name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The namespace parent id
        /// </summary>
        public NamespaceId ParentId { get; }

        public override string ToString()
        {
            return $"{nameof(NamespaceId)}: {NamespaceId}, {nameof(Name)}: {Name}, {nameof(ParentId)}: {ParentId}";
        }
    }
}