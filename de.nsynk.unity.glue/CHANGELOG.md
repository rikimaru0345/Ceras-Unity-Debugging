# Glue
## 0.1.8
- Bug Fixing
  - Catch OutOfRange exceptions in material receiver and VFX graph receiver (bool/float)

## 0.1.7
- Receivers: reflection probe
    - Conversion from SharpDX was missing
    - Add controlable multiplier

## 0.1.6
- GlueMeter for debugging in screen
- Receiver/Rendering:
    - Reflection Probe control receiver

## 0.1.5
- Receiver/Timeline:
    - Ensure time of 0 on start
- Glue.dll/Ceras
    - Official bug fix in Ceras 4.1.6:
      https://github.com/rikimaru0345/Ceras/issues/62

## 0.1.4
- GlueInspector
    - Showing the Array size before each element in [] brackets
- Glue.dll
    - Hot fix for sending larger Boolean arrays (encoding Boolean values to bytes under the hood)
    - Bug history traceable here:
      https://github.com/rikimaru0345/Ceras/issues/62
    - Still waiting for an official fix in Ceras library
- Light Receiver
    - Changed to sending 6 float values rather than structured Vector4s @dima
    - Added color temperature control

## 0.1.3
- GlueInspector: 
    - Measured time between receivedFrame and late update
    - Improved UI
- GlueSimpleBehavior: behaviour with manually managed keys + overwrites
- GlueSettings: bundle all settings into on data block
- GlueDataPool: singleton for getting data from Glue
- VFX Graph receiver: first version
- Material value receivers: 
    - Fix material slot index access
    - Check has property block before getting it from the renderer
    - Dont evaluate any further if no material slot was set
- Utils: add method for casting Vector2 (SharpDX to Unity)

## 0.1.2
- Fix serialization bug for Use shared material flag on material value receivers

## 0.1.1
- Added array for materialSlots, for animating meshes with more than one material

## 0.1.0
- Removed Handshake from GlueConnector
- Global.Verbose to check and set if the Debug log should be spammed
- Changed default ports
- Improved general stability

## 0.0.16
- ReceiveCamera has public _unityCamera propert for interpolation with other
  Unity cameras
- FIX/Receive Light: use HDR intensity rather than the old school intensity
- FIX: Receive Toggle Game Objects
- FEATURE/Scenager: overwrite Glue values in play mode

## 0.0.15
- First version of a Timeline receiver

## 0.0.14
- New receiver scripts for material values (reworked using material property
  blocks)
- Scenemanager (Scenager) UI rework

## 0.0.13
- ReceiveMaterialValue rewörk 

## 0.0.12
- Scenager: first version of scene managing via Glue — Thanks @Dima
- Glue Receive Material Value: basic usage of material property blocks

## 0.0.11
- Fix bug when using overwrite mechanism for ReciveLight
- Glue Receive Material Value: Use sharedMaterial in Editor mode always

## 0.0.10
- Temporary overwrite mechanism for receiver scripts
- Improved Glue Inspector (concatenated array strings)
- Wrap editor scripts for proper building of Unity projects

## 0.0.9
- all Receivers work on Arrays of values now
- and wrap just like vvvv spreads
- also ReceiveMaterialValue sharedMaterial toggle

## 0.0.8
- add ReceiveTransform + ReceiveTransform [Children]
- add ReceiveScale [Children]
- improved Receiver caching yadayada

## 0.0.7
- FIX: add missing .dll (System.Runtime.CompilerService.Unsafe) - Trying out Ceras AoT feature

## 0.0.6
- FIX bug with Recive Material Values To Childer (.material was used instead of
  .sharedMaterial which lead to material instancing all the time)
- Receive spread scripts
- Receive light scripts
