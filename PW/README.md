## PW介绍

本程序所做的事：在右键菜单中 <mark>为压缩包或其他文件添加密码</mark> 或者 <mark>获取压缩包密码</mark>
已废弃，有更好的程序代替它了

本程序为 `Microsoft.NETCore.App` 控制台程序

运行依赖`.NET6.0`

运行平台`Windows`


  

## 集成到右键菜单

1. 得到源代码编译出的<mark> .exe </mark>文件路径(exefilepath)

2. 打开注册表编辑器(先备份)

3. 搜索`HKEY_CLASSES_ROOT\*\shell`

4. 在此位置右键单击“shell”，然后选择“新建” > “项” 并为新项命名，例如“为该文件添加密码”

5. 在该项下添加子项 命名为"command"

6. 为子项设置默认值

7. ```text
   exefilepath W "%1"
   ```

8. exefilepath 需替换为本地文件路径

9. 重复第4步骤 命名为“显示并复制密码”

10. 在该项下添加子项 命名为"command"

11. 为子项设置默认值

12. ```text
    exefilepath R "%1"
    ```

13. exefilepath 需替换为本地文件路径

14. 保存更改，至此已完成

