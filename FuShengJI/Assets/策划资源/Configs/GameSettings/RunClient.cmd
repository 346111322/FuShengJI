echo off
echo Copy to Client
copy .\Chinese.txt ..\..\..\Client\Trunk\Assets\Resources\Common\Configs\Localization\Chinese.txt
copy .\FilterWords.txt ..\..\..\Client\Trunk\Assets\Resources\Common\Configs\FilterWord\FilterWords.txt
copy .\GameSettings.txt ..\..\..\Client\Trunk\Assets\Resources\Common\Configs\Setting\Setting.txt

echo ======== 导出成功 ========
pause