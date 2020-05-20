#! /bin/sh -

rm -rf */obj
rm -rf */bin
rm -rf obj bin
rm -rf .vs
rm -rf packages
rm -rf "$LOCALAPPDATA/Microsoft/VisualStudio/16.0"*"Exp/Extensions"

while test $# -gt 0
do
    case "$1" in
        --all)
	    rm -rf "$LOCALAPPDATA/Microsoft/VisualStudio/16.0"*"Exp"
            ;;
    esac
    shift
done

