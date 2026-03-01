using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PawnshopSimulator;
using PawnshopSimulator.Audio;
using PawnshopSimulator.Building;
using PawnshopSimulator.Characters;
using PawnshopSimulator.Customers;
using PawnshopSimulator.Data;
using PawnshopSimulator.MainMenu;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.UI
{
    [AddComponentMenu("UI/Effects/4 Corners Gradient")]
    public class UICornersGradient : BaseMeshEffect {
    	public Color m_topLeftColor = Color.white;
    	public Color m_topRightColor = Color.white;
    	public Color m_bottomRightColor = Color.white;
    	public Color m_bottomLeftColor = Color.white;

        public override void ModifyMesh(VertexHelper vh)
        {
    		if(enabled)
    		{
                Rect rect = graphic.rectTransform.rect;
    			UIGradientUtils.Matrix2x3 localPositionMatrix = UIGradientUtils.LocalPositionMatrix(rect, Vector2.right);

    			UIVertex vertex = default(UIVertex);
    			for (int i = 0; i < vh.currentVertCount; i++) {
    				vh.PopulateUIVertex (ref vertex, i);
    				Vector2 normalizedPosition = localPositionMatrix * vertex.position;
    				vertex.color *= UIGradientUtils.Bilerp(m_bottomLeftColor, m_bottomRightColor, m_topLeftColor, m_topRightColor, normalizedPosition);
    				vh.SetUIVertex (vertex, i);
    			}
    		}
        }
    }
}
