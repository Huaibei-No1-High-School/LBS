import os
namelist = os.listdir("pic")
print(namelist)
cnt = 0
for i in namelist:
    t = i.split(".")
    os.rename("pic\\" + i,"pic\\" + str(cnt)+"."+t[1])
    cnt = cnt + 1;
