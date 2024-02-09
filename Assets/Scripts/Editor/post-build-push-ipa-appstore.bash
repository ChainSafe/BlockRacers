 #!/bin/bash 
echo "Uploading IPA to Appstore Connect..."
 
#Path is "/BUILD_PATH/<ORG_ID>.<PROJECT_ID>.<BUILD_TARGET_ID>/.build/last/<BUILD_TARGET_ID>/build.ipa"


if xcrun altool --upload-app -f "/BUILD_PATH/kantagara.block-racers.block-racers-ios/.build/last/block-racers-ios/build.ipa" --type ios -u $ITUNES_USERNAME -p $ITUNES_PASSWORD ; then
    echo "Upload IPA to Appstore Connect finished with success"
else
    echo "Upload IPA to Appstore Connect failed"
fi