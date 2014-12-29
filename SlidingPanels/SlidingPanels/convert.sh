#!/bin/sh
if test x$1 = x; then
    echo usage is: convert-to-unified [--bold] foo.csproj
    echo Use the flag --bold if you are extra bold, and want to get some common UITableView conversions
fi
bold=false
if test x$1 = x--bold; then
    bold=true
    shift
fi

function upf()
{
   echo SUP $1

   echo DONE
}

perl -pi -e 's/6BC8ED88-2882-458C-8E55-DFD12B67127B/FEACFBD2-3405-455C-9665-78FE426C6842/' $1
perl -pi -e 's/Reference Include="monotouch"/Reference Include="Xamarin.iOS"/' $1
perl -pi -e 's,(<InterfaceDefinition Include.*) xmlns="",\1,' $1
perl -pi -e 's,<Import Project="\$\(MSBuildBinPath\)\\Microsoft.CSharp.targets",<Import Project="\$\(MSBuildExtensionsPath\)\\Xamarin\\iOS\\Xamarin.iOS.CSharp.targets",' $1
grep 'Compile Include' $1  | sed -e 's/.*="//' -e 's/".*//' -e 's,\\,/,' | while read a; do 
perl -pi -e 's/MonoTouch\.//;' -e 's/System.Drawing/CoreGraphics/g;' -e 's/RectangleF/CGRect/g;' -e 's/PointF/CGPoint/g;' -e 's/SizeF/CGSize/g;' -e 's/DismissModalViewControllerAnimated/DismissModalViewController/;' $a
if $bold; then
  perl -pi -e 's/public override int NumberOfSections \(UITableView tableView\)/public override nint NumberOfSections \(UITableView tableView\)/' $a
  perl -pi -e 's/public override string TitleForHeader \(UITableView tableView, int section\)/public override string TitleForHeader \(UITableView tableView, nint section\)/' $a
  perl -pi -e 's/public override int RowsInSection \(UITableView tableView, int section\)/public override nint RowsInSection \(UITableView tableView, nint section\)/' $a
  perl -pi -e 's/public override float GetHeightForRow \(UITableView tableView, NSIndexPath indexPath\)/public override nfloat GetHeightForRow \(UITableView tableView, NSIndexPath indexPath\)/' $a

fi
done

