# Synthetic dataset generation for 3D light estimation
A unity project that can be used to automatically create a dataset of images of simple but realistic CGI rendered objects in a outdoor scene, along with their lighting informations. The lighting is composed only of an infinitely far away directional light (such as the sun). </br>
The object masks are also produced for segmentation purposes.

The generated dataset with this code can be freely downloaded here [SynthOutdoor-dataset](https://www.scidb.cn/en/detail?dataSetId=304a5d88dba04226957b6215c171c0c2).


<p float="center">
  <img src="./imgs/syn1.png" width="250" />
  <img src="./imgs/syn2.png" width="250" /> 
  <img src="./imgs/syn3.png" width="250" />
</p>

<p float="center">
  <img src="./imgs/normals1.png" width="250" />
  <img src="./imgs/normals2.png" width="250" /> 
  <img src="./imgs/normals3.png" width="250" />
</p>

<p float="center">
  <img src="./imgs/mask1.png" width="250" />
  <img src="./imgs/mask2.png" width="250" /> 
  <img src="./imgs/mask3.png" width="250" />
</p>

<p float="center">
  <img src="./imgs/light1.png" width="250" />
  <img src="./imgs/light2.png" width="250" /> 
  <img src="./imgs/light3.png" width="250" />
</p>


## How to run the project and generate the dataset
1. [Download Unity 3d](https://unity.com/products) and install it.
2. Clone this repository. From a Powershell or Cmd terminal (Windows) or Bash shell (Linux), run the command:
   ```
   git clone https://github.com/marza1993/SynthOutdoor.git
   ```
3. Import this project inside Unity.
4. Create an empty root folder `<choose_your_root_folder_of_dataset>` that will contain the generated dataset, *E.g.,: SynthOutdoor*. Inside this folder, create three sub-folders: *images*, *normals* and *masks*.
5. In the `Assets\scripts\GameHandler.cs`, modify the output path variables according to the created folder structure in the previous point:
    ```
    public static string img_path = @"<choose_your_root_folder_of_dataset>\images\";
    public static string mask_path = @"<choose_your_root_folder_of_dataset>\masks\";
    public static string normals_path = @"<choose_your_root_folder_of_dataset>\normals\";
    public static string fileName = @"<choose_your_root_folder_of_dataset>\light.csv";
    ```
 6. Run the code from the Unity interface.

## Acknowledgments
If you intend to use this code in a research paper, you must cite our work as:


```
@article{SynthOutdoor,
title = {SynthOutdoor: A synthetic dataset for 3D outdoor light estimation},
journal = {Data in Brief},
volume = {55},
pages = {110700},
year = {2024},
issn = {2352-3409},
doi = {https://doi.org/10.1016/j.dib.2024.110700},
url = {https://www.sciencedirect.com/science/article/pii/S235234092400667X},
author = {Marcello Zanardelli and Mahyar G. Moghaddam and Riccardo Leonardi and Sergio Benini and Nicola Adami},
keywords = {3D light estimation, Dataset, Computer graphics, Unity3D},
abstract = {In this work, we present a novel dataset, SynthOutdoor, comprising 39,086 high-resolution images, aimed at addressing the data scarcity in the field of 3D light direction estimation under the assumption of distant lighting. SynthOutdoor was generated using our software (which is also publicly available), that in turn is based on the Unity3D engine. Our dataset provides a set of images rendered from a given input scene, with the camera moving across a predefined path within the scene. This dataset captures a wide variety of lighting conditions through the implementation of a solar cycle. The dataset's ground truth is composed of the following elements: the 3D light direction and color intensity of the sun; the color intensity of the ambient light; the instance segmentation masks of each object and the surface normals map, in which each pixel is assigned with the 3D surface normal in that point (encoded as 3 color channels). By providing not only the light direction and intensity, but also the geometric and semantic information of the rendered images, our dataset can be used not only for light estimation, but also for more general tasks such as 3D geometry and shading estimation from 2D images.}
}
```


