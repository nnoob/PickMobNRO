using System;
using Assets.src.g;

namespace Assets.src.f
{
	internal class Controller2
	{
		public static void readMessage(Message msg)
		{
			try
			{
				Res.outz("cmd=" + msg.command);
				switch (msg.command)
				{
				case sbyte.MaxValue:
					readInfoRada(msg);
					break;
				case 113:
				{
					int loop = msg.reader().readByte();
					int layer = msg.reader().readByte();
					int id2 = msg.reader().readUnsignedByte();
					short x = msg.reader().readShort();
					short y = msg.reader().readShort();
					short loopCount = msg.reader().readShort();
					EffecMn.addEff(new Effect(id2, x, y, layer, loop, loopCount));
					break;
				}
				case 48:
				{
					sbyte b33 = msg.reader().readByte();
					ServerListScreen.ipSelect = b33;
					GameCanvas.instance.doResetToLoginScr(GameCanvas.serverScreen);
					Session_ME.gI().close();
					GameCanvas.endDlg();
					ServerListScreen.waitToLogin = true;
					break;
				}
				case 31:
				{
					int num12 = msg.reader().readInt();
					sbyte b13 = msg.reader().readByte();
					if (b13 == 1)
					{
						short smallID = msg.reader().readShort();
						sbyte b14 = -1;
						int[] array = null;
						short wimg = 0;
						short himg = 0;
						try
						{
							b14 = msg.reader().readByte();
							if (b14 > 0)
							{
								sbyte b15 = msg.reader().readByte();
								array = new int[b15];
								for (int n = 0; n < b15; n++)
								{
									array[n] = msg.reader().readByte();
								}
								wimg = msg.reader().readShort();
								himg = msg.reader().readShort();
							}
						}
						catch (Exception)
						{
						}
						if (num12 == Char.myCharz().charID)
						{
							Char.myCharz().petFollow = new PetFollow();
							Char.myCharz().petFollow.smallID = smallID;
							if (b14 > 0)
							{
								Char.myCharz().petFollow.SetImg(b14, array, wimg, himg);
							}
							break;
						}
						Char @char = GameScr.findCharInMap(num12);
						@char.petFollow = new PetFollow();
						@char.petFollow.smallID = smallID;
						if (b14 > 0)
						{
							@char.petFollow.SetImg(b14, array, wimg, himg);
						}
					}
					else if (num12 == Char.myCharz().charID)
					{
						Char.myCharz().petFollow.remove();
						Char.myCharz().petFollow = null;
					}
					else
					{
						Char char2 = GameScr.findCharInMap(num12);
						char2.petFollow.remove();
						char2.petFollow = null;
					}
					break;
				}
				case -89:
					GameCanvas.open3Hour = ((msg.reader().readByte() == 1) ? true : false);
					break;
				case 42:
				{
					GameCanvas.endDlg();
					LoginScr.isContinueToLogin = false;
					Char.isLoadingMap = false;
					sbyte haveName = msg.reader().readByte();
					if (GameCanvas.registerScr == null)
					{
						GameCanvas.registerScr = new RegisterScreen(haveName);
					}
					GameCanvas.registerScr.switchToMe();
					break;
				}
				case 52:
				{
					sbyte b27 = msg.reader().readByte();
					if (b27 == 1)
					{
						int num31 = msg.reader().readInt();
						if (num31 == Char.myCharz().charID)
						{
							Char.myCharz().setMabuHold(m: true);
							Char.myCharz().cx = msg.reader().readShort();
							Char.myCharz().cy = msg.reader().readShort();
						}
						else
						{
							Char char4 = GameScr.findCharInMap(num31);
							if (char4 != null)
							{
								char4.setMabuHold(m: true);
								char4.cx = msg.reader().readShort();
								char4.cy = msg.reader().readShort();
							}
						}
					}
					if (b27 == 0)
					{
						int num32 = msg.reader().readInt();
						if (num32 == Char.myCharz().charID)
						{
							Char.myCharz().setMabuHold(m: false);
						}
						else
						{
							GameScr.findCharInMap(num32)?.setMabuHold(m: false);
						}
					}
					if (b27 == 2)
					{
						int charId2 = msg.reader().readInt();
						int id3 = msg.reader().readInt();
						Mabu mabu = (Mabu)GameScr.findCharInMap(charId2);
						mabu.eat(id3);
					}
					if (b27 == 3)
					{
						GameScr.mabuPercent = msg.reader().readByte();
					}
					break;
				}
				case 51:
				{
					int charId3 = msg.reader().readInt();
					Mabu mabu2 = (Mabu)GameScr.findCharInMap(charId3);
					sbyte id4 = msg.reader().readByte();
					short x2 = msg.reader().readShort();
					short y2 = msg.reader().readShort();
					sbyte b31 = msg.reader().readByte();
					Char[] array10 = new Char[b31];
					int[] array11 = new int[b31];
					for (int num42 = 0; num42 < b31; num42++)
					{
						int num43 = msg.reader().readInt();
						Res.outz("char ID=" + num43);
						array10[num42] = null;
						if (num43 != Char.myCharz().charID)
						{
							array10[num42] = GameScr.findCharInMap(num43);
						}
						else
						{
							array10[num42] = Char.myCharz();
						}
						array11[num42] = msg.reader().readInt();
					}
					mabu2.setSkill(id4, x2, y2, array10, array11);
					break;
				}
				case -127:
					readLuckyRound(msg);
					break;
				case -126:
				{
					sbyte b22 = msg.reader().readByte();
					Res.outz("type quay= " + b22);
					if (b22 == 1)
					{
						sbyte b23 = msg.reader().readByte();
						string num22 = msg.reader().readUTF();
						string finish = msg.reader().readUTF();
						GameScr.gI().showWinNumber(num22, finish);
					}
					if (b22 == 0)
					{
						GameScr.gI().showYourNumber(msg.reader().readUTF());
					}
					break;
				}
				case -122:
				{
					short id = msg.reader().readShort();
					Npc npc = GameScr.findNPCInMap(id);
					sbyte b6 = msg.reader().readByte();
					npc.duahau = new int[b6];
					Res.outz("N DUA HAU= " + b6);
					for (int l = 0; l < b6; l++)
					{
						npc.duahau[l] = msg.reader().readShort();
					}
					npc.setStatus(msg.reader().readByte(), msg.reader().readInt());
					break;
				}
				case 102:
				{
					sbyte b24 = msg.reader().readByte();
					if (b24 == 0 || b24 == 1 || b24 == 2 || b24 == 6)
					{
						BigBoss2 bigBoss = Mob.getBigBoss2();
						if (bigBoss == null)
						{
							break;
						}
						if (b24 == 6)
						{
							bigBoss.x = (bigBoss.y = (bigBoss.xTo = (bigBoss.yTo = (bigBoss.xFirst = (bigBoss.yFirst = -1000)))));
							break;
						}
						sbyte b25 = msg.reader().readByte();
						Char[] array3 = new Char[b25];
						int[] array4 = new int[b25];
						for (int num27 = 0; num27 < b25; num27++)
						{
							int num28 = msg.reader().readInt();
							array3[num27] = null;
							if (num28 != Char.myCharz().charID)
							{
								array3[num27] = GameScr.findCharInMap(num28);
							}
							else
							{
								array3[num27] = Char.myCharz();
							}
							array4[num27] = msg.reader().readInt();
						}
						bigBoss.setAttack(array3, array4, b24);
					}
					if (b24 != 3 && b24 != 4 && b24 != 5 && b24 != 7)
					{
						break;
					}
					BachTuoc bachTuoc = Mob.getBachTuoc();
					if (bachTuoc == null)
					{
						break;
					}
					if (b24 == 7)
					{
						bachTuoc.x = (bachTuoc.y = (bachTuoc.xTo = (bachTuoc.yTo = (bachTuoc.xFirst = (bachTuoc.yFirst = -1000)))));
						break;
					}
					if (b24 == 3 || b24 == 4)
					{
						sbyte b26 = msg.reader().readByte();
						Char[] array5 = new Char[b26];
						int[] array6 = new int[b26];
						for (int num29 = 0; num29 < b26; num29++)
						{
							int num30 = msg.reader().readInt();
							array5[num29] = null;
							if (num30 != Char.myCharz().charID)
							{
								array5[num29] = GameScr.findCharInMap(num30);
							}
							else
							{
								array5[num29] = Char.myCharz();
							}
							array6[num29] = msg.reader().readInt();
						}
						bachTuoc.setAttack(array5, array6, b24);
					}
					if (b24 == 5)
					{
						short xMoveTo = msg.reader().readShort();
						bachTuoc.move(xMoveTo);
					}
					break;
				}
				case 101:
				{
					Res.outz("big boss--------------------------------------------------");
					BigBoss bigBoss2 = Mob.getBigBoss();
					if (bigBoss2 == null)
					{
						break;
					}
					sbyte b28 = msg.reader().readByte();
					if (b28 == 0 || b28 == 1 || b28 == 2 || b28 == 4 || b28 == 3)
					{
						if (b28 == 3)
						{
							bigBoss2.xTo = (bigBoss2.xFirst = msg.reader().readShort());
							bigBoss2.yTo = (bigBoss2.yFirst = msg.reader().readShort());
							bigBoss2.setFly();
						}
						else
						{
							sbyte b29 = msg.reader().readByte();
							Res.outz("CHUONG nChar= " + b29);
							Char[] array7 = new Char[b29];
							int[] array8 = new int[b29];
							for (int num33 = 0; num33 < b29; num33++)
							{
								int num34 = msg.reader().readInt();
								Res.outz("char ID=" + num34);
								array7[num33] = null;
								if (num34 != Char.myCharz().charID)
								{
									array7[num33] = GameScr.findCharInMap(num34);
								}
								else
								{
									array7[num33] = Char.myCharz();
								}
								array8[num33] = msg.reader().readInt();
							}
							bigBoss2.setAttack(array7, array8, b28);
						}
					}
					if (b28 == 5)
					{
						bigBoss2.haftBody = true;
						bigBoss2.status = 2;
					}
					if (b28 == 6)
					{
						bigBoss2.getDataB2();
						bigBoss2.x = msg.reader().readShort();
						bigBoss2.y = msg.reader().readShort();
					}
					if (b28 == 7)
					{
						bigBoss2.setAttack(null, null, b28);
					}
					if (b28 == 8)
					{
						bigBoss2.xTo = (bigBoss2.xFirst = msg.reader().readShort());
						bigBoss2.yTo = (bigBoss2.yFirst = msg.reader().readShort());
						bigBoss2.status = 2;
					}
					if (b28 == 9)
					{
						bigBoss2.x = (bigBoss2.y = (bigBoss2.xTo = (bigBoss2.yTo = (bigBoss2.xFirst = (bigBoss2.yFirst = -1000)))));
					}
					break;
				}
				case -120:
				{
					long num38 = mSystem.currentTimeMillis();
					Service.logController = num38 - Service.curCheckController;
					Service.gI().sendCheckController();
					break;
				}
				case -121:
				{
					long num23 = mSystem.currentTimeMillis();
					Service.logMap = num23 - Service.curCheckMap;
					Service.gI().sendCheckMap();
					break;
				}
				case 100:
				{
					sbyte b10 = msg.reader().readByte();
					sbyte b11 = msg.reader().readByte();
					Item item = null;
					if (b10 == 0)
					{
						item = Char.myCharz().arrItemBody[b11];
					}
					if (b10 == 1)
					{
						item = Char.myCharz().arrItemBag[b11];
					}
					short num10 = msg.reader().readShort();
					if (num10 == -1)
					{
						break;
					}
					item.template = ItemTemplates.get(num10);
					item.quantity = msg.reader().readInt();
					item.info = msg.reader().readUTF();
					item.content = msg.reader().readUTF();
					sbyte b12 = msg.reader().readByte();
					if (b12 == 0)
					{
						break;
					}
					item.itemOption = new ItemOption[b12];
					for (int m = 0; m < item.itemOption.Length; m++)
					{
						int num11 = msg.reader().readUnsignedByte();
						Res.outz("id o= " + num11);
						int param2 = msg.reader().readUnsignedShort();
						if (num11 != -1)
						{
							item.itemOption[m] = new ItemOption(num11, param2);
						}
					}
					break;
				}
				case -123:
				{
					int charId = msg.reader().readInt();
					if (GameScr.findCharInMap(charId) != null)
					{
						GameScr.findCharInMap(charId).perCentMp = msg.reader().readByte();
					}
					break;
				}
				case -119:
					Char.myCharz().rank = msg.reader().readInt();
					break;
				case -117:
					GameScr.gI().tMabuEff = 0;
					GameScr.gI().percentMabu = msg.reader().readByte();
					if (GameScr.gI().percentMabu == 100)
					{
						GameScr.gI().mabuEff = true;
					}
					if (GameScr.gI().percentMabu == 101)
					{
						Npc.mabuEff = true;
					}
					break;
				case -116:
					GameScr.canAutoPlay = ((msg.reader().readByte() == 1) ? true : false);
					break;
				case -115:
					Char.myCharz().setPowerInfo(msg.reader().readUTF(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort());
					break;
				case -113:
				{
					sbyte[] array2 = new sbyte[5];
					for (int num24 = 0; num24 < 5; num24++)
					{
						array2[num24] = msg.reader().readByte();
						Res.outz("vlue i= " + array2[num24]);
					}
					GameScr.gI().onKSkill(array2);
					GameScr.gI().onOSkill(array2);
					GameScr.gI().onCSkill(array2);
					break;
				}
				case -111:
				{
					short num25 = msg.reader().readShort();
					ImageSource.vSource = new MyVector();
					for (int num26 = 0; num26 < num25; num26++)
					{
						string iD = msg.reader().readUTF();
						sbyte version = msg.reader().readByte();
						ImageSource.vSource.addElement(new ImageSource(iD, version));
					}
					ImageSource.checkRMS();
					ImageSource.saveRMS();
					break;
				}
				case 125:
				{
					sbyte fusion = msg.reader().readByte();
					int num21 = msg.reader().readInt();
					if (num21 == Char.myCharz().charID)
					{
						Char.myCharz().setFusion(fusion);
					}
					else if (GameScr.findCharInMap(num21) != null)
					{
						GameScr.findCharInMap(num21).setFusion(fusion);
					}
					break;
				}
				case 124:
				{
					short num35 = msg.reader().readShort();
					string text3 = msg.reader().readUTF();
					Res.outz("noi chuyen = " + text3 + "npc ID= " + num35);
					GameScr.findNPCInMap(num35)?.addInfo(text3);
					break;
				}
				case 123:
				{
					Res.outz("SET POSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSss");
					int num45 = msg.reader().readInt();
					short xPos = msg.reader().readShort();
					short yPos = msg.reader().readShort();
					sbyte b32 = msg.reader().readByte();
					Char char5 = null;
					if (num45 == Char.myCharz().charID)
					{
						char5 = Char.myCharz();
					}
					else if (GameScr.findCharInMap(num45) != null)
					{
						char5 = GameScr.findCharInMap(num45);
					}
					if (char5 != null)
					{
						ServerEffect.addServerEffect((b32 != 0) ? 173 : 60, char5, 1);
						char5.setPos(xPos, yPos, b32);
					}
					break;
				}
				case 122:
				{
					short num44 = msg.reader().readShort();
					Res.outz("second login = " + num44);
					LoginScr.timeLogin = num44;
					LoginScr.currTimeLogin = (LoginScr.lastTimeLogin = mSystem.currentTimeMillis());
					GameCanvas.endDlg();
					break;
				}
				case 121:
					mSystem.publicID = msg.reader().readUTF();
					mSystem.strAdmob = msg.reader().readUTF();
					Res.outz("SHOW AD public ID= " + mSystem.publicID);
					mSystem.createAdmob();
					break;
				case -124:
				{
					sbyte b7 = msg.reader().readByte();
					sbyte b8 = msg.reader().readByte();
					if (b8 == 0)
					{
						if (b7 == 2)
						{
							int num4 = msg.reader().readInt();
							if (num4 == Char.myCharz().charID)
							{
								Char.myCharz().removeEffect();
							}
							else if (GameScr.findCharInMap(num4) != null)
							{
								GameScr.findCharInMap(num4).removeEffect();
							}
						}
						int num5 = msg.reader().readUnsignedByte();
						int num6 = msg.reader().readInt();
						if (num5 == 32)
						{
							if (b7 == 1)
							{
								int num7 = msg.reader().readInt();
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().holdEffID = num5;
									GameScr.findCharInMap(num7).setHoldChar(Char.myCharz());
								}
								else if (GameScr.findCharInMap(num6) != null && num7 != Char.myCharz().charID)
								{
									GameScr.findCharInMap(num6).holdEffID = num5;
									GameScr.findCharInMap(num7).setHoldChar(GameScr.findCharInMap(num6));
								}
								else if (GameScr.findCharInMap(num6) != null && num7 == Char.myCharz().charID)
								{
									GameScr.findCharInMap(num6).holdEffID = num5;
									Char.myCharz().setHoldChar(GameScr.findCharInMap(num6));
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeHoleEff();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeHoleEff();
							}
						}
						if (num5 == 33)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().protectEff = true;
								}
								else if (GameScr.findCharInMap(num6) != null)
								{
									GameScr.findCharInMap(num6).protectEff = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeProtectEff();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeProtectEff();
							}
						}
						if (num5 == 39)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().huytSao = true;
								}
								else if (GameScr.findCharInMap(num6) != null)
								{
									GameScr.findCharInMap(num6).huytSao = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeHuytSao();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeHuytSao();
							}
						}
						if (num5 == 40)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().blindEff = true;
								}
								else if (GameScr.findCharInMap(num6) != null)
								{
									GameScr.findCharInMap(num6).blindEff = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeBlindEff();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeBlindEff();
							}
						}
						if (num5 == 41)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().sleepEff = true;
								}
								else if (GameScr.findCharInMap(num6) != null)
								{
									GameScr.findCharInMap(num6).sleepEff = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().removeSleepEff();
							}
							else if (GameScr.findCharInMap(num6) != null)
							{
								GameScr.findCharInMap(num6).removeSleepEff();
							}
						}
						if (num5 == 42)
						{
							if (b7 == 1)
							{
								if (num6 == Char.myCharz().charID)
								{
									Char.myCharz().stone = true;
								}
							}
							else if (num6 == Char.myCharz().charID)
							{
								Char.myCharz().stone = false;
							}
						}
					}
					if (b8 != 1)
					{
						break;
					}
					int num8 = msg.reader().readUnsignedByte();
					sbyte b9 = msg.reader().readByte();
					Res.outz("modbHoldID= " + b9 + " skillID= " + num8 + "eff ID= " + b7);
					if (num8 == 32)
					{
						if (b7 == 1)
						{
							int num9 = msg.reader().readInt();
							if (num9 == Char.myCharz().charID)
							{
								GameScr.findMobInMap(b9).holdEffID = num8;
								Char.myCharz().setHoldMob(GameScr.findMobInMap(b9));
							}
							else if (GameScr.findCharInMap(num9) != null)
							{
								GameScr.findMobInMap(b9).holdEffID = num8;
								GameScr.findCharInMap(num9).setHoldMob(GameScr.findMobInMap(b9));
							}
						}
						else
						{
							GameScr.findMobInMap(b9).removeHoldEff();
						}
					}
					if (num8 == 40)
					{
						if (b7 == 1)
						{
							GameScr.findMobInMap(b9).blindEff = true;
						}
						else
						{
							GameScr.findMobInMap(b9).removeBlindEff();
						}
					}
					if (num8 == 41)
					{
						if (b7 == 1)
						{
							GameScr.findMobInMap(b9).sleepEff = true;
						}
						else
						{
							GameScr.findMobInMap(b9).removeSleepEff();
						}
					}
					break;
				}
				case -125:
				{
					ChatTextField.gI().isShow = false;
					string text = msg.reader().readUTF();
					Res.outz("titile= " + text);
					sbyte b4 = msg.reader().readByte();
					ClientInput.gI().setInput(b4, text);
					for (int k = 0; k < b4; k++)
					{
						ClientInput.gI().tf[k].name = msg.reader().readUTF();
						sbyte b5 = msg.reader().readByte();
						if (b5 == 0)
						{
							ClientInput.gI().tf[k].setIputType(TField.INPUT_TYPE_NUMERIC);
						}
						if (b5 == 1)
						{
							ClientInput.gI().tf[k].setIputType(TField.INPUT_TYPE_ANY);
						}
						if (b5 == 2)
						{
							ClientInput.gI().tf[k].setIputType(TField.INPUT_TYPE_PASSWORD);
						}
					}
					break;
				}
				case -110:
				{
					sbyte b30 = msg.reader().readByte();
					if (b30 == 1)
					{
						int num39 = msg.reader().readInt();
						sbyte[] array9 = Rms.loadRMS(num39 + string.Empty);
						if (array9 == null)
						{
							Service.gI().sendServerData(1, -1, null);
						}
						else
						{
							Service.gI().sendServerData(1, num39, array9);
						}
					}
					if (b30 == 0)
					{
						int num40 = msg.reader().readInt();
						short num41 = msg.reader().readShort();
						sbyte[] data = new sbyte[num41];
						msg.reader().read(ref data, 0, num41);
						Rms.saveRMS(num40 + string.Empty, data);
					}
					break;
				}
				case 93:
				{
					string str = msg.reader().readUTF();
					str = Res.changeString(str);
					GameScr.gI().chatVip(str);
					break;
				}
				case -106:
				{
					short num36 = msg.reader().readShort();
					int num37 = msg.reader().readShort();
					if (ItemTime.isExistItem(num36))
					{
						ItemTime.getItemById(num36).initTime(num37);
						break;
					}
					ItemTime o = new ItemTime(num36, num37);
					Char.vItemTime.addElement(o);
					break;
				}
				case -105:
					TransportScr.gI().time = 0;
					TransportScr.gI().maxTime = msg.reader().readShort();
					TransportScr.gI().last = (TransportScr.gI().curr = mSystem.currentTimeMillis());
					TransportScr.gI().type = msg.reader().readByte();
					TransportScr.gI().switchToMe();
					break;
				case -103:
				{
					sbyte b17 = msg.reader().readByte();
					if (b17 == 0)
					{
						GameCanvas.panel.vFlag.removeAllElements();
						sbyte b18 = msg.reader().readByte();
						for (int num13 = 0; num13 < b18; num13++)
						{
							Item item2 = new Item();
							short num14 = msg.reader().readShort();
							if (num14 != -1)
							{
								item2.template = ItemTemplates.get(num14);
								sbyte b19 = msg.reader().readByte();
								if (b19 != -1)
								{
									item2.itemOption = new ItemOption[b19];
									for (int num15 = 0; num15 < item2.itemOption.Length; num15++)
									{
										int num16 = msg.reader().readUnsignedByte();
										int param3 = msg.reader().readUnsignedShort();
										if (num16 != -1)
										{
											item2.itemOption[num15] = new ItemOption(num16, param3);
										}
									}
								}
							}
							GameCanvas.panel.vFlag.addElement(item2);
						}
						GameCanvas.panel.setTypeFlag();
						GameCanvas.panel.show();
					}
					else if (b17 == 1)
					{
						int num17 = msg.reader().readInt();
						sbyte b20 = msg.reader().readByte();
						Res.outz("---------------actionFlag1:  " + num17 + " : " + b20);
						if (num17 == Char.myCharz().charID)
						{
							Char.myCharz().cFlag = b20;
						}
						else if (GameScr.findCharInMap(num17) != null)
						{
							GameScr.findCharInMap(num17).cFlag = b20;
						}
						GameScr.gI().getFlagImage(num17, b20);
					}
					else
					{
						if (b17 != 2)
						{
							break;
						}
						sbyte b21 = msg.reader().readByte();
						int num18 = msg.reader().readShort();
						PKFlag pKFlag = new PKFlag();
						pKFlag.cflag = b21;
						pKFlag.IDimageFlag = num18;
						GameScr.vFlag.addElement(pKFlag);
						for (int num19 = 0; num19 < GameScr.vFlag.size(); num19++)
						{
							PKFlag pKFlag2 = (PKFlag)GameScr.vFlag.elementAt(num19);
							Res.outz("i: " + num19 + "  cflag: " + pKFlag2.cflag + "   IDimageFlag: " + pKFlag2.IDimageFlag);
						}
						for (int num20 = 0; num20 < GameScr.vCharInMap.size(); num20++)
						{
							Char char3 = (Char)GameScr.vCharInMap.elementAt(num20);
							if (char3 != null && char3.cFlag == b21)
							{
								char3.flagImage = num18;
							}
						}
						if (Char.myCharz().cFlag == b21)
						{
							Char.myCharz().flagImage = num18;
						}
					}
					break;
				}
				case -102:
				{
					sbyte b16 = msg.reader().readByte();
					if (b16 != 0 && b16 == 1)
					{
						GameCanvas.loginScr.isLogin2 = false;
						Service.gI().login(Rms.loadRMSString("acc"), Rms.loadRMSString("pass"), GameMidlet.VERSION, 0);
						LoginScr.isLoggingIn = true;
					}
					break;
				}
				case -101:
				{
					GameCanvas.loginScr.isLogin2 = true;
					GameCanvas.connect();
					string text2 = msg.reader().readUTF();
					Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, text2);
					Service.gI().setClientType();
					Service.gI().login(text2, string.Empty, GameMidlet.VERSION, 1);
					break;
				}
				case -100:
				{
					InfoDlg.hide();
					bool flag = false;
					if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
					{
						flag = true;
					}
					sbyte b = msg.reader().readByte();
					Res.outz("t Indxe= " + b);
					GameCanvas.panel.maxPageShop[b] = msg.reader().readByte();
					GameCanvas.panel.currPageShop[b] = msg.reader().readByte();
					Res.outz("max page= " + GameCanvas.panel.maxPageShop[b] + " curr page= " + GameCanvas.panel.currPageShop[b]);
					int num = msg.reader().readUnsignedByte();
					Char.myCharz().arrItemShop[b] = new Item[num];
					for (int i = 0; i < num; i++)
					{
						short num2 = msg.reader().readShort();
						if (num2 == -1)
						{
							continue;
						}
						Res.outz("template id= " + num2);
						Char.myCharz().arrItemShop[b][i] = new Item();
						Char.myCharz().arrItemShop[b][i].template = ItemTemplates.get(num2);
						Char.myCharz().arrItemShop[b][i].itemId = msg.reader().readShort();
						Char.myCharz().arrItemShop[b][i].buyCoin = msg.reader().readInt();
						Char.myCharz().arrItemShop[b][i].buyGold = msg.reader().readInt();
						Char.myCharz().arrItemShop[b][i].buyType = msg.reader().readByte();
						Char.myCharz().arrItemShop[b][i].quantity = msg.reader().readByte();
						Char.myCharz().arrItemShop[b][i].isMe = msg.reader().readByte();
						Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
						sbyte b2 = msg.reader().readByte();
						if (b2 != -1)
						{
							Char.myCharz().arrItemShop[b][i].itemOption = new ItemOption[b2];
							for (int j = 0; j < Char.myCharz().arrItemShop[b][i].itemOption.Length; j++)
							{
								int num3 = msg.reader().readUnsignedByte();
								int param = msg.reader().readUnsignedShort();
								if (num3 != -1)
								{
									Char.myCharz().arrItemShop[b][i].itemOption[j] = new ItemOption(num3, param);
									Char.myCharz().arrItemShop[b][i].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[b][i]);
								}
							}
						}
						sbyte b3 = msg.reader().readByte();
						if (b3 == 1)
						{
							int headTemp = msg.reader().readShort();
							int bodyTemp = msg.reader().readShort();
							int legTemp = msg.reader().readShort();
							int bagTemp = msg.reader().readShort();
							Char.myCharz().arrItemShop[b][i].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
						}
					}
					if (flag)
					{
						GameCanvas.panel2.setTabKiGui();
					}
					GameCanvas.panel.setTabShop();
					GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
					break;
				}
				}
			}
			catch (Exception)
			{
			}
		}

		private static void readLuckyRound(Message msg)
		{
			try
			{
				sbyte b = msg.reader().readByte();
				if (b == 0)
				{
					sbyte b2 = msg.reader().readByte();
					short[] array = new short[b2];
					for (int i = 0; i < b2; i++)
					{
						array[i] = msg.reader().readShort();
					}
					sbyte b3 = msg.reader().readByte();
					int price = msg.reader().readInt();
					short idTicket = msg.reader().readShort();
					CrackBallScr.gI().SetCrackBallScr(array, (byte)b3, price, idTicket);
				}
				else if (b == 1)
				{
					sbyte b4 = msg.reader().readByte();
					short[] array2 = new short[b4];
					for (int j = 0; j < b4; j++)
					{
						array2[j] = msg.reader().readShort();
					}
					CrackBallScr.gI().DoneCrackBallScr(array2);
				}
			}
			catch (Exception)
			{
			}
		}

		private static void readInfoRada(Message msg)
		{
			try
			{
				sbyte b = msg.reader().readByte();
				if (b == 0)
				{
					RadarScr.gI();
					MyVector myVector = new MyVector(string.Empty);
					short num = msg.reader().readShort();
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						Info_RadaScr info_RadaScr = new Info_RadaScr();
						int id = msg.reader().readShort();
						int no = i + 1;
						int idIcon = msg.reader().readShort();
						sbyte rank = msg.reader().readByte();
						sbyte amount = msg.reader().readByte();
						sbyte max_amount = msg.reader().readByte();
						short templateId = -1;
						Char charInfo = null;
						sbyte b2 = msg.reader().readByte();
						if (b2 == 0)
						{
							templateId = msg.reader().readShort();
						}
						else
						{
							int head = msg.reader().readShort();
							int body = msg.reader().readShort();
							int leg = msg.reader().readShort();
							int bag = msg.reader().readShort();
							charInfo = Info_RadaScr.SetCharInfo(head, body, leg, bag);
						}
						string name = msg.reader().readUTF();
						string info = msg.reader().readUTF();
						sbyte b3 = msg.reader().readByte();
						sbyte use = msg.reader().readByte();
						sbyte b4 = msg.reader().readByte();
						ItemOption[] array = null;
						if (b4 != 0)
						{
							array = new ItemOption[b4];
							for (int j = 0; j < array.Length; j++)
							{
								int num3 = msg.reader().readUnsignedByte();
								int param = msg.reader().readUnsignedShort();
								sbyte activeCard = msg.reader().readByte();
								if (num3 != -1)
								{
									array[j] = new ItemOption(num3, param);
									array[j].activeCard = activeCard;
								}
							}
						}
						info_RadaScr.SetInfo(id, no, idIcon, rank, b2, templateId, name, info, charInfo, array);
						info_RadaScr.SetLevel(b3);
						info_RadaScr.SetUse(use);
						info_RadaScr.SetAmount(amount, max_amount);
						myVector.addElement(info_RadaScr);
						if (b3 > 0)
						{
							num2++;
						}
					}
					RadarScr.gI().SetRadarScr(myVector, num2, num);
					RadarScr.gI().switchToMe();
				}
				else if (b == 1)
				{
					int id2 = msg.reader().readShort();
					sbyte use2 = msg.reader().readByte();
					if (Info_RadaScr.GetInfo(RadarScr.list, id2) != null)
					{
						Info_RadaScr.GetInfo(RadarScr.list, id2).SetUse(use2);
					}
					RadarScr.SetListUse();
				}
				else if (b == 2)
				{
					int num4 = msg.reader().readShort();
					sbyte level = msg.reader().readByte();
					int num5 = 0;
					for (int k = 0; k < RadarScr.list.size(); k++)
					{
						Info_RadaScr info_RadaScr2 = (Info_RadaScr)RadarScr.list.elementAt(k);
						if (info_RadaScr2 != null)
						{
							if (info_RadaScr2.id == num4)
							{
								info_RadaScr2.SetLevel(level);
							}
							if (info_RadaScr2.level > 0)
							{
								num5++;
							}
						}
					}
					RadarScr.SetNum(num5, RadarScr.list.size());
					if (Info_RadaScr.GetInfo(RadarScr.listUse, num4) != null)
					{
						Info_RadaScr.GetInfo(RadarScr.listUse, num4).SetLevel(level);
					}
				}
				else if (b == 3)
				{
					int id3 = msg.reader().readShort();
					sbyte amount2 = msg.reader().readByte();
					sbyte max_amount2 = msg.reader().readByte();
					if (Info_RadaScr.GetInfo(RadarScr.list, id3) != null)
					{
						Info_RadaScr.GetInfo(RadarScr.list, id3).SetAmount(amount2, max_amount2);
					}
					if (Info_RadaScr.GetInfo(RadarScr.listUse, id3) != null)
					{
						Info_RadaScr.GetInfo(RadarScr.listUse, id3).SetAmount(amount2, max_amount2);
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
