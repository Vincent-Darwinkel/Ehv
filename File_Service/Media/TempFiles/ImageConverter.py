import os
from PIL import Image
currentPath = os.path.dirname(os.path.realpath(__file__)) + "/"
supportedFileTypes = [".webp", ".jpg", "jpeg", ".png"]
fileName = ""

for type in supportedFileTypes:
    if (os.path.isfile(currentPath + "input" + type)):
        fileName = "input" + type
        break

im = Image.open(currentPath + fileName).convert("RGB")
im.save(currentPath + "output.webp", "webp", quality=65)