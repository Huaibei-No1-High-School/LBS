import os
import win32clipboard as clip
import win32con
from PIL import Image
from io import StringIO
from io import BytesIO
from aip import AipOcr

APP_ID = '17942189'
API_KEY = 'dD4bDfmKj9QGvGz84XvVLwaV'
SECRET_KEY = '7siy7rVkth8UKsB3dO3syZ8zXj1fmgRc'

client = AipOcr(APP_ID, API_KEY, SECRET_KEY)

'''
    往剪贴板中放入图片
'''
def setImage(imagepath):
    img = Image.open(imagepath)# Image.open可以打开网络图片与本地图片。
    output = BytesIO()  # BytesIO实现了在内存中读写bytes
    img.convert("RGB").save(output, "BMP") #以RGB模式保存图像
    data = output.getvalue()[14:]
    output.close()
    setImage(data)
    clip.OpenClipboard() #打开剪贴板
    clip.EmptyClipboard()  #先清空剪贴板
    clip.SetClipboardData(win32con.CF_DIB, data)  #将图片放入剪贴板
    clip.CloseClipboard()

def readimg(path):
    with open(path,'rb') as fp:
        return fp.read()


if __name__ == '__main__':
    namelist = os.listdir("pic");
    print(namelist)
