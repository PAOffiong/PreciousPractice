using System.Collections;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Java.Lang;

namespace PreciousPractice.Platforms.Android;

public class InstantPasswordTransformationMethod : PasswordTransformationMethod
{
    public override ICharSequence GetTransformationFormatted(ICharSequence source, global::Android.Views.View view)
    {
        if (source == null)
            return new Java.Lang.String(string.Empty);

        return new BulletCharSequence(source);
    }

    private class BulletCharSequence : Java.Lang.Object, ICharSequence
    {
        private readonly ICharSequence _source;

        public BulletCharSequence(ICharSequence source)
        {
            _source = source;
        }

        public char CharAt(int index) => '\u2022';

        public int Length() => _source.Length();

        public ICharSequence SubSequenceFormatted(int start, int end) =>
            _source.SubSequenceFormatted(start, end);

        public override string ToString()
        {
            return new string('\u2022', _source.Length());
        }

        public IEnumerator<char> GetEnumerator()
        {
            for (int i = 0; i < Length(); i++)
                yield return '\u2022';
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}