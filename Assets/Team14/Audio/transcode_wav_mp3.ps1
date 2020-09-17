while ( $deleteInp -notmatch "[yYnN]" ) { 
    $deleteInp = Read-Host -Prompt "Delete files when done? [y/n]"
}

$delete = $deleteInp -match "[yY]"
$deleteInp = "" # reset this since its global


echo "------ Transcoding MP3s -------"
$oldFileEnding='.wav'
$newFileEnding='.mp3'
Get-ChildItem -File -Recurse | Where-Object { $_.FullName.EndsWith($oldFileEnding) } | ForEach-Object {
  $old=$_.FullName
  $new = $old.Substring(0, $old.Length - $oldFileEnding.Length) + $newFileEnding
  
  ffmpeg -i $old $new
  if ($delete) {
    echo "Deleting WAV $old"
    Remove-Item $old
  }
}

echo "------ Renaming metas -------"
$oldFileEnding='.wav.meta'
$newFileEnding='.mp3.meta'
Get-ChildItem -File -Recurse | Where-Object { $_.FullName.EndsWith($oldFileEnding) } | ForEach-Object {
  $old=$_.FullName
  $new = $old.Substring(0, $old.Length - $oldFileEnding.Length) + $newFileEnding
  
  Copy-Item $old $new
  if ($delete) {
    echo "Deleting meta $old"
    Remove-Item $old
  }
}