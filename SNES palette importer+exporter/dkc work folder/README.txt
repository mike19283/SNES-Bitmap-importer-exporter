
 _____ _   _  _____ _____           ______ _ _                                            
/  ___| \ | ||  ___/  ___|          | ___ (_) |                                           
\ `--.|  \| || |__ \ `--.   ______  | |_/ /_| |_ _ __ ___   __ _ _ __                     
 `--. \ . ` ||  __| `--. \ |______| | ___ \ | __| '_ ` _ \ / _` | '_ \                    
/\__/ / |\  || |___/\__/ /          | |_/ / | |_| | | | | | (_| | |_) |                   
\____/\_| \_/\____/\____/           \____/|_|\__|_| |_| |_|\__,_| .__/                    
                                                                | |                       
                                                                |_|                       
 _____                      _              _______                           _            
|  ___|                    | |            / /_   _|                         | |           
| |____  ___ __   ___  _ __| |_ ___ _ __ / /  | | _ __ ___  _ __   ___  _ __| |_ ___ _ __ 
|  __\ \/ / '_ \ / _ \| '__| __/ _ \ '__/ /   | || '_ ` _ \| '_ \ / _ \| '__| __/ _ \ '__|
| |___>  <| |_) | (_) | |  | ||  __/ | / /   _| || | | | | | |_) | (_) | |  | ||  __/ |   
\____/_/\_\ .__/ \___/|_|   \__\___|_|/_/    \___/_| |_| |_| .__/ \___/|_|   \__\___|_|   
          | |                                              | |                            
          |_|                                              |_|                            
                                                                                                 
------------------------

Check out this companion video to the readme! https://www.youtube.com/watch?v=oIcW81WBVSk

(1) Requirements:


SNES Rom
Image Editor (Gimp Recommended):	https://www.gimp.org/
An emulator http://tasvideos.org/EmulatorResources.html

(2) About:

SNES - Bitmap Exporter/Importer is a C# program written by Mike (RainbowSprinklez). This was inspired by a lua script by Alex Corley (H4v0c21). It's designed to make editing SNES palettes easy. After opening a ROM, the program has the ability to convert 256 bytes or 128 SNES colors to 24 bit RGB and export the data as a 16(*20 for ease of viewing) x 8(*20 for ease of viewing) bitmap file. Each set of 400 pixels in the bitmap file represents 1 SNES color. This file can be opened in an image editor of your choice. Please note that the only image editor tested was "Gimp" but this should work in any image editor that can export .bmp files. This tool was designed for 256 byte level palettes in DKC 1 but will work with other DKC Games. If you have any issues, requests, or questions please reach out to me at Rainbow#2405 on discord or mikemingrone@gmail.com for email.

(3) How to Use:

WARNING!!! This will write to anywhere you specify in rom. If the wrong address is specified it could overwrite important game data. Use at your own risk and make a backup first.

Disclaimer: The SNES cannot produce all 16.7 million RGB 24 bit colors. The SNES supports 15 bit color (only 32,768 colors).
Any changes in color when converting back to SNES format are due to the SNES's color limitations and are not the fault of this program. All colors converted to 24 bit RGB should be represented correctly and convert back to SNES without any changes.

1. Loading a ROM
This will recognize any file that is either a .smc or.sfc. You could have either an sfc or smc. sfc is the standard/official (Super FamiCom) ROM, and shouldn't have a 512-byte header at the beginning of the file. smc is an unofficial but popular extension (Super MagiCom, a copier), and will probably have a 512-byte header at the beginning of the file. You should already be using a headerless ROM, but just in case, this removes the header for you automatically on load and overwrites your ROM with a headerless one.

2. Exporting the palette to bitmap.
To export the palette just input the offset in the textbox. I have included a list of the dkc1 offsets from Giangurgolo's docs. For other games you'll have to find them on your own for now. Remember, 0x is assumed and shouldn't be input. Once you have entered the address press EXPORT and a save dialog appears. This is to save your bitmap. You can now open this in your image editor and begin editing. Now you can use whatever tools you want to change the colors. Once you're done editing open the File Dropdown Menu and select "Overwrite (filename)"..

This also works with objects. There are 15 color palettes per object. This *might* be DKC1 + 2 + 3 exclusive. In this case, a smaller palette is exported.

3. Importing the bitmap to rom.
To import the palette to rom go back to the program and select IMPORT. *NOTE* The address used is the address already in the textbox (see step 2 if needed). After clicking import the program will open a file selection screen to select a bitmap to use. You will get a preview, and your bitmap will be saved to ROM. The amount of data copied is only based on the size of the bitmap, therefore, if you import a bg bitmap into an object's slot, you run the risk of breaking things.

*IMPORTANT*
In order to make palettes visible, each is a 20 by 20 square. Only the pixel in the top left of a color is read/written for import/export.

(4) Sharing and Distribution:
You are free to share and distribute this script freely. I would prefer if any modified versions of it be shared with me and credit to H4v0c21's youtube must be included. 

https://www.youtube.com/channel/UCVOb0O8Jvgab-1qc8YEjhpA
