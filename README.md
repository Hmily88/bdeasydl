# bdeasydl
百度easydl图像检测和识别模型软件/一键打出报告
软件支持图像分类和物体检测两种模型

# 使用方式
在百度easydl平台训练模型【https://ai.baidu.com/easydl/】  
1.先在百度easydl平台完成【图像分类】或【物体检测】的模型训练后，平台生成SDK包，完成后打包SDK软件到本地  
2.获取sdk序列号，准备用来激活sdk  
3.启动SDK，输入序列号，设置sdk的服务地址和端口  
4.打开本软件，设置地址和服务端口  
5.读取图片开始检测识别


# 提示
只要是图像检测和图像分类的模型都可以使用，下面的图片是我自己训练做的例子，如果你训练的模型和我的不一样也可以使用  
我自己练的是芯片，你可以自己练动物识别，手势识别等等，最后都能在软件上呈现效果。  


# 主界面
![1](https://user-images.githubusercontent.com/75898236/194805264-a0b6e632-a9f3-4e36-beef-fc215dc1397e.png)



# 设置服务地址
![2](https://user-images.githubusercontent.com/75898236/194805278-ddc74cc4-3bb6-469b-85c7-4e2b919c630a.png)



# 报告生成
打印报告后会在指定位置输出doc文档
![image](https://user-images.githubusercontent.com/75898236/194805351-d8b98dc9-d7e3-4361-a856-9c88cbe9edbe.png)

![image](https://user-images.githubusercontent.com/75898236/194805370-9a80d291-c4e4-4d67-bbd2-22eeec40e5ab.png)



# 报告模板
模板文件在/WinFormDemo1/template.docx 
可以自行更改模板内容，生成后可以按照该模板样式替换检测的图片文本  
template.docx文件设置了需要和软件放在同一目录下，自己生成后放一起就行了，自己也可以在源码里面更改
![image](https://user-images.githubusercontent.com/75898236/194805695-b4239d87-51b3-4c46-b33b-89d21dc0bdc8.png)


# VX:ZWL10203040
