import os
import re
import random
from PIL import Image
import numpy as np

mask_path = "./masks"
img_path = "./images"
normals_path = "./normals"

regex_img = r'.+\.(png|jpg)$'

removed_dir ="./removed"
if not os.path.exists(removed_dir):
    os.makedirs(removed_dir)

list_masks = [f for f in os.listdir(mask_path) if re.search(regex_img,f)]
print("found {} masks..".format(len(list_masks)))

cont_removed = 0

for i,mask_name in enumerate(list_masks):
    
    print("processing img {} of {}".format(i, len(list_masks)))
    
    mask = Image.open(os.path.join(mask_path, mask_name))
    mask = np.array(mask)[...,:-1] # escludo il canale alpha
    
    mask_non_white = mask != (255,255,255)
    
    
    if not mask_non_white.any():
        
        print("rimossa una maschera")
        cont_removed += 1
        # sposto la maschera
        os.rename(os.path.join(mask_path, mask_name), os.path.join(removed_dir, mask_name))
        
        img_name = mask_name.replace("mask_","")
        
        # sposto l'immagine corrispondente
        os.rename(os.path.join(img_path, img_name), os.path.join(removed_dir, img_name))

        normal_img_name = mask_name.replace("mask_", "normals_")

        # sposto l'immagine delle normali corrispondente
        os.rename(os.path.join(normals_path, normal_img_name), os.path.join(removed_dir, normal_img_name))
        
   
print("removed {} images and masks".format(cont_removed))