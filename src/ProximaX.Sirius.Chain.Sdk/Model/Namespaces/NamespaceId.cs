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

using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Namespaces
{
    /// <summary>
    ///     The namespace identifier
    /// </summary>
    public class NamespaceId
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NamespaceId" /> class.
        /// </summary>
        /// <param name="name">The namespace name</param>
        public NamespaceId(string name)
        {
            var nsPath = IdGenerator.GenerateNamespacePath(name);
            Id = nsPath[nsPath.Count - 1];
            Name = name;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NamespaceId" /> class.
        /// </summary>
        /// <param name="id"></param>
        public NamespaceId(ulong id)
        {
            Id = id;
            Name = null;
        }

        /// <summary>
        ///     The namespace id
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        ///     The namespace name
        /// </summary>
        public string Name { get; }

        public string HexId => Id.ToHex();

        /// <summary>
        ///     Helper method to generate sub namespace from parent
        /// </summary>
        /// <param name="subNamespace"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static NamespaceId CreateFromParent(string subNamespace, NamespaceId parentId)
        {
            var id = IdGenerator.GenerateSubNamespaceIdFromParentId(parentId.Id, subNamespace);
            return new NamespaceId(id);
        }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(HexId)}: {HexId}";
        }
    }
}