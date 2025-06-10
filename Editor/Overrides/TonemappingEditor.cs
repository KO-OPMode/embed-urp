using UnityEngine.Rendering.Universal;

namespace UnityEditor.Rendering.Universal
{
    [CustomEditor(typeof(Tonemapping))]
    sealed class TonemappingEditor : VolumeComponentEditor
    {
        SerializedDataParameter m_Mode;

        // HDR Mode.
        SerializedDataParameter m_NeutralHDRRangeReductionMode;
        SerializedDataParameter m_HueShiftAmount;
        SerializedDataParameter m_HDRDetectPaperWhite;
        SerializedDataParameter m_HDRPaperwhite;
        SerializedDataParameter m_HDRDetectNitLimits;
        SerializedDataParameter m_HDRMinNits;
        SerializedDataParameter m_HDRMaxNits;
        SerializedDataParameter m_HDRAcesPreset;
        
        // ys custom start
        SerializedDataParameter m_AgXLook;
        SerializedDataParameter m_AgXCustomOffset;
        SerializedDataParameter m_AgXCustomSlope;
        SerializedDataParameter m_AgXCustomPower;
        SerializedDataParameter m_AgXCustomSaturation;
        // ys custom end

        public override bool hasAdditionalProperties => true;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<Tonemapping>(serializedObject);

            m_Mode = Unpack(o.Find(x => x.mode));
            m_NeutralHDRRangeReductionMode = Unpack(o.Find(x => x.neutralHDRRangeReductionMode));
            m_HueShiftAmount = Unpack(o.Find(x => x.hueShiftAmount));
            m_HDRDetectPaperWhite = Unpack(o.Find(x => x.detectPaperWhite));
            m_HDRPaperwhite = Unpack(o.Find(x => x.paperWhite));
            m_HDRDetectNitLimits = Unpack(o.Find(x => x.detectBrightnessLimits));
            m_HDRMinNits = Unpack(o.Find(x => x.minNits));
            m_HDRMaxNits = Unpack(o.Find(x => x.maxNits));
            m_HDRAcesPreset = Unpack(o.Find(x => x.acesPreset));
            
            // ys custom start
            m_AgXLook =  Unpack(o.Find(x => x.agXLook));
            m_AgXCustomOffset =  Unpack(o.Find(x => x.agXCustomOffset));
            m_AgXCustomSlope =  Unpack(o.Find(x => x.agXCustomSlope));
            m_AgXCustomPower =  Unpack(o.Find(x => x.agXCustomPower));
            m_AgXCustomSaturation =  Unpack(o.Find(x => x.agXCustomSaturation));
            // ys custom end
        }

        public override void OnInspectorGUI()
        {
            PropertyField(m_Mode);

            // Display a warning if the user is trying to use a tonemap while rendering in LDR
            var asset = UniversalRenderPipeline.asset;
            int hdrTonemapMode = m_Mode.value.intValue;
            if (asset != null && !asset.supportsHDR && hdrTonemapMode != (int)TonemappingMode.None)
            {
                EditorGUILayout.HelpBox("Tonemapping should only be used when working with High Dynamic Range (HDR). Please enable HDR through the active Render Pipeline Asset.", MessageType.Warning);
                return;
            }
            
            // ys custom start
            if (hdrTonemapMode == (int)TonemappingMode.AgX)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("AgX Look Settings");

                PropertyField(m_AgXLook);

                if (m_AgXLook.value.intValue == (int)AgXLook.Custom)
                {
                    EditorGUI.indentLevel++;
                    PropertyField(m_AgXCustomOffset);
                    PropertyField(m_AgXCustomSlope);
                    PropertyField(m_AgXCustomPower);
                    PropertyField(m_AgXCustomSaturation);
                    EditorGUI.indentLevel--;
                }
            }
            // ys custom end

            if (PlayerSettings.allowHDRDisplaySupport && hdrTonemapMode != (int)TonemappingMode.None)
            {
                EditorGUILayout.LabelField("HDR Output");

                if (hdrTonemapMode == (int)TonemappingMode.Neutral)
                {
                    PropertyField(m_NeutralHDRRangeReductionMode);
                    PropertyField(m_HueShiftAmount);

                    PropertyField(m_HDRDetectPaperWhite);
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(m_HDRDetectPaperWhite.value.boolValue))
                    {
                        PropertyField(m_HDRPaperwhite);
                    }
                    EditorGUI.indentLevel--;

                    PropertyField(m_HDRDetectNitLimits);
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(m_HDRDetectNitLimits.value.boolValue))
                    {
                        PropertyField(m_HDRMinNits);
                        PropertyField(m_HDRMaxNits);
                    }
                    EditorGUI.indentLevel--;
                }
                if (hdrTonemapMode == (int)TonemappingMode.ACES)
                {
                    PropertyField(m_HDRAcesPreset);

                    PropertyField(m_HDRDetectPaperWhite);
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(m_HDRDetectPaperWhite.value.boolValue))
                    {
                        PropertyField(m_HDRPaperwhite);
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }
    }
}
