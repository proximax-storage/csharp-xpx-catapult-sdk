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

using System.Collections.Generic;
using ProximaX.Sirius.Sdk.Model.Accounts;

namespace ProximaX.Sirius.Sdk.Model.Namespaces
{
    /// <summary>
    ///     Namespace information
    /// </summary>
    public class NamespaceInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NamespaceInfo" /> class.
        /// </summary>
        /// <param name="active">The namespace is active</param>
        /// <param name="index">The namespace index</param>
        /// <param name="metaId">The namespace metaId</param>
        /// <param name="type">The namespace type, namespace and sub namespace.</param>
        /// <param name="depth">The namespace depth</param>
        /// <param name="levels">The namespace level</param>
        /// <param name="parentId">The namespace parent</param>
        /// <param name="owner">The owner of the namespace</param>
        /// <param name="startHeight">The height at which the ownership begins.</param>
        /// <param name="endHeight">The height at which the ownership ends.</param>
        /// <param name="alias"> The alias linked to a namespace.</param>
        public NamespaceInfo(bool? active, int? index, string metaId, NamespaceType type, int? depth,
            List<NamespaceId> levels, NamespaceId parentId, PublicAccount owner, ulong startHeight, ulong endHeight,
            Alias alias)
        {
            Active = active;
            Index = index;
            MetaId = metaId;
            Type = type;
            Depth = depth;
            Levels = levels;
            ParentId = parentId;
            Owner = owner;
            StartHeight = startHeight;
            EndHeight = endHeight;
            Alias = alias;
        }

        /// <summary>
        ///     Namespace is active.
        /// </summary>
        public bool? Active { get; }

        /// <summary>
        ///     The namespace index
        /// </summary>
        public int? Index { get; }

        /// <summary>
        ///     The meta data id.
        /// </summary>
        public string MetaId { get; }

        /// <summary>
        ///     The namespace type, namespace and sub namespace.
        /// </summary>
        public NamespaceType Type { get; }

        /// <summary>
        ///     The depth of namespace.
        /// </summary>
        public int? Depth { get; }

        /// <summary>
        ///     The namespace id levels.
        /// </summary>
        public List<NamespaceId> Levels { get; }

        /// <summary>
        ///     The namespace parent id.
        /// </summary>
        public NamespaceId ParentId { get; }

        /// <summary>
        ///     The owner of the namespace.
        /// </summary>
        public PublicAccount Owner { get; }

        /// <summary>
        ///     The height at which the ownership begins.
        /// </summary>
        public ulong StartHeight { get; }

        /// <summary>
        ///     The height at which the ownership ends.
        /// </summary>
        public ulong EndHeight { get; }

        /// <summary>
        ///     The alias linked to a namespace.
        /// </summary>
        public Alias Alias { get; }

        /// <summary>
        ///     Is root namespace
        /// </summary>
        /// <returns></returns>
        public bool IsRoot => Type == NamespaceType.ROOT_NAMESPACE;

        /// <summary>
        ///     Is sub namespace
        /// </summary>
        public bool IsSubNamespace => Type == NamespaceType.SUB_NAMESPACE;

        /// <summary>
        ///     Has alias
        /// </summary>
        public bool HasAlias => Alias.Type != AliasType.NO_ALIAS;

        /// <summary>
        ///     The parent namespace
        /// </summary>
        public NamespaceId ParentNamespace => IsRoot ? null : ParentId;

        /// <summary>
        ///     The namespace id
        /// </summary>
        public NamespaceId Id => Levels[Levels.Count - 1];

        public override string ToString()
        {
            return
                $"{nameof(Active)}: {Active}, {nameof(Index)}: {Index}, {nameof(MetaId)}: {MetaId}, {nameof(Type)}: {Type}, {nameof(Depth)}: {Depth}, {nameof(Levels)}: {Levels}, {nameof(ParentId)}: {ParentId}, {nameof(Owner)}: {Owner}, {nameof(StartHeight)}: {StartHeight}, {nameof(EndHeight)}: {EndHeight}, {nameof(Alias)}: {Alias}, {nameof(IsRoot)}: {IsRoot}, {nameof(IsSubNamespace)}: {IsSubNamespace}, {nameof(HasAlias)}: {HasAlias}, {nameof(ParentNamespace)}: {ParentNamespace}, {nameof(Id)}: {Id}";
        }
    }
}