using Cysharp.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/*--------脚本描述-----------
电子邮箱：
    2605218730@qq.com
作者:
    琉璃无常
描述:
    主角管理类
-----------------------*/

namespace MyFrameworkCore
{
    public class Player : MonoSingleton<Player>
    {
        public StateMachine playerStateMachine { get; set; }
        public InputActions inputActions { get; set; }
        public Rigidbody2D rb { get; set; }
        public Animator[] animators { get; set; }

        private bool isMoving = false;
        public Vector2 inputDirection;
        public float speed = 10f;
        public void Awake()
        {
            //组件获取
            rb = gameObject.GetComponent<Rigidbody2D>();
            animators = GetComponentsInChildren<Animator>();
            //玩家输入
            inputActions = new InputActions();
            inputActions.Enable();
            inputActions.Player.Move.performed += OnMovePreformed;
            inputActions.Player.Move.canceled += OnMoveCanceled;



            playerStateMachine = new StateMachine(this);
            playerStateMachine.AddNode<FsmPlayerIdle>(inputActions,rb);
            playerStateMachine.AddNode<FsmPlayerWalk>(inputActions,rb);

        }

        void Start()
        {
            playerStateMachine.Run<FsmPlayerIdle>();
            ConfigEvent.PlayerMoveToPosition.AddEventListener<Vector3>(OnMoveToPosition);
        }
        void Update()
        {
            
            playerStateMachine.Update();
            UpdateMoveAnimation();
        }
         void FixedUpdate()
        {
            rb.velocity = inputDirection * speed;
        }

        /// <summary> 按下按键时移动 </summary>
        public void OnMovePreformed(InputAction.CallbackContext context)
        {
            isMoving = true;
            print("按下按键了"+context.ReadValue<Vector2>());
            inputDirection = context.ReadValue<Vector2>();
        }

        /// <summary> 松开按键时停止移动 </summary>
        public void OnMoveCanceled(InputAction.CallbackContext context)
        {
            isMoving = false;
            print("松开了" + context.ReadValue<Vector2>());
            inputDirection = context.ReadValue<Vector2>();
        }

        /// <summary> 刷新移动动画 </summary>
        public void UpdateMoveAnimation()
        {
            float inputX = inputDirection.x;
            float inputY = inputDirection.y;

            foreach (var anim in animators)
            {
                anim.SetBool("IsMoving", isMoving);
                if (isMoving)
                {
                    anim.SetFloat("InputX", inputX);
                    anim.SetFloat("InputY", inputY);
                }
            }
        }
        /// <summary> 设置人物坐标位置 </summary>
        private void OnMoveToPosition(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }
    }
}
