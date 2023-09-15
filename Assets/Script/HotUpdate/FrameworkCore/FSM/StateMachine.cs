using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    状态机
-----------------------*/

namespace MyFrameworkCore
{
    public class StateMachine
    {
        private readonly Dictionary<string, IStateNode> dictNodes = new Dictionary<string, IStateNode>();
        private IStateNode curNode;
        private IStateNode preNode;

        public System.Object Owner { private set; get; }

        public StateMachine(object owner)
        {
            this.Owner = owner;
        }
        public void Update()
        {
            if(curNode != null)
            {
                curNode.OnUpdate();
            }
        }

        public void Run<TNode>() where TNode : IStateNode
        {
            var nodeType = typeof(TNode);
            var nodeName = nodeType.FullName;
            Run(nodeName);
        }
        public void Run(string entryNode)
        {
            curNode = TryGetNode(entryNode);
            preNode = curNode;

            if (curNode == null)
                throw new Exception($"没有找到进入节点: {entryNode}");

            curNode.OnEnter();
        }
        public void AddNode<TNode>() where TNode: IStateNode
        {
            var nodeType = typeof(TNode);
            var stateNode = Activator.CreateInstance(nodeType) as IStateNode;
            if (stateNode == null)
                throw new ArgumentNullException();

            var nodeName = nodeType.FullName;

            if (!dictNodes.ContainsKey(nodeName))
            {
                stateNode.OnCreate(this);
                dictNodes.Add(nodeName, stateNode);
            }
            else
            {
                Debug.Log($"当前节点已存在:{nodeName}");
            }
        }
        public void AddNode<TNode>(InputActions inputAssets) where TNode : IStateNode
        {
            var nodeType = typeof(TNode);
            var stateNode = Activator.CreateInstance(nodeType) as IStateNode;
            if (stateNode == null)
                throw new ArgumentNullException();

            var nodeName = nodeType.FullName;

            if (!dictNodes.ContainsKey(nodeName))
            {
                stateNode.OnCreate(this, inputAssets);
                dictNodes.Add(nodeName, stateNode);
            }
            else
            {
                Debug.Log($"当前节点已存在:{nodeName}");
            }
        }
        public void AddNode<TNode>(InputActions inputAssets,Rigidbody2D rb) where TNode : IStateNode
        {
            var nodeType = typeof(TNode);
            var stateNode = Activator.CreateInstance(nodeType) as IStateNode;
            if (stateNode == null)
                throw new ArgumentNullException();

            var nodeName = nodeType.FullName;

            if (!dictNodes.ContainsKey(nodeName))
            {
                stateNode.OnCreate(this, inputAssets,rb);
                dictNodes.Add(nodeName, stateNode);
            }
            else
            {
                Debug.Log($"当前节点已存在:{nodeName}");
            }
        }

        public void ChangeState<TNode>() where TNode : IStateNode
        {
            var nodeType = typeof(TNode);
            var nodeName = nodeType.FullName;
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException();

            IStateNode node = TryGetNode(nodeName);
            if (node == null)
            {
                Debug.Log($"要转换的下一个节点没有找到:{nodeName}");
                return;
            }
            Debug.Log($"{curNode.GetType().FullName} --> {node.GetType().FullName}");
            preNode = curNode;
            curNode.OnExit();
            curNode = node;
            curNode.OnEnter();
        }

        private IStateNode TryGetNode(string nodeName)
        {
            dictNodes.TryGetValue(nodeName, out IStateNode node);
            return node;
        }

    }
}
